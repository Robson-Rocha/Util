using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace RobsonROX.Util.Extensions
{
    /// <summary>
    /// Métodos de extensão para Strings
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Testa se a string representa um e-mail válido
        /// </summary>
        /// <param name="email">String que será testada</param>
        /// <returns>Booleano representando se a string é válida (true) ou não (false)</returns>
        public static bool IsEMail(this string email)
        {
            const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" 
                                   + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" 
                                   + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(pattern, RegexOptions.IgnoreCase).IsMatch(email);
        }

        /// <summary>
        /// Pluraliza um gtexto singular em inglês
        /// </summary>
        /// <param name="text">Texto singular</param>
        /// <returns>Texto plural</returns>
        public static string Pluralize(this string text)
        {
            var service = Singleton<EnglishPluralizationService>.Instance;
            return service.IsSingular(text) ? service.Pluralize(text) : text;
        }

        /// <summary>
        /// Singulariza um texto plural em inglês
        /// </summary>
        /// <param name="text">Texto plural</param>
        /// <returns>Texto singular</returns>
        public static string Singularize(this string text)
        {
            var service = Singleton<EnglishPluralizationService>.Instance;
            return service.IsPlural(text) ? service.Singularize(text) : text;
        }

        static readonly string[] WordsExclusion =
        {
            "de",
            "da",
            "do",
            "o",
            "a",
            "e",
            "ou",
            "ate",
            "em",
            "seu",
            "sua",
        };

        /// <summary>
        /// Capitaliza um texto
        /// </summary>
        /// <param name="text">Texto a ser capitalizado</param>
        /// <returns>Texto capitalizado</returns>
        public static string Capitalize(this string text)
        {
            text = text.Trim();
            int num;
            if (text.Length <= 0) return "";
            string initial = text.Substring(0, 1).ToUpper();
            if (int.TryParse(initial, out num)) initial = "_" + initial;
            return initial + text.Substring(1).ToLower();
        }

        /// <summary>
        /// Ger um Slug a partir de um texto
        /// </summary>
        /// <param name="text">Texto a ser convertido em slug</param>
        /// <returns>Slug gerado</returns>
        public static string ToSlug(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";

            //Remove acentuações
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            text = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

            //Converte ALLCAPS em Allcaps
            text = new Regex("([A-Z]+)").Replace(text, m => Capitalize(m.ToString()));

            //Separa CamelCase em Camel Case
            text = Regex.Replace(text, "([A-Z])", " $1", RegexOptions.Compiled).Trim();

            //Remove caracteres não alfanuméricos
            text = Regex.Replace(text, @"[^a-zA-Z0-9]", " ", RegexOptions.Compiled);

            //Remove espaços duplicados
            text = Regex.Replace(text, "\\s+", " ", RegexOptions.Compiled).Trim();

            //Separa as palavras por espaços, remove as palavras da lista de exclusão, e monta um CamelCase
            var words = text.Split(' ').ToArray();
            return string.Join(" ", words.Where(w => !WordsExclusion.Contains(w.ToLowerInvariant().Trim())).Select(Capitalize)).Replace(" ", "");
        }


        /// <summary>
        /// Retorna uma hash MD5 a partir do texto fornecido
        /// </summary>
        /// <param name="input">Texto a ser gerada a hash</param>
        /// <returns>Hash MD5 gerada</returns>
        public static string ToMd5(this string input)
        {
            MD5 md5 = MD5.Create();
            var sb = new StringBuilder();
            foreach (byte t in md5.ComputeHash(Encoding.ASCII.GetBytes(input)))
            {
                sb.Append(t.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Retorna uma hash SHA256 a partir do texto fornecido
        /// </summary>
        /// <param name="input">Texto a ser gerada a hash</param>
        /// <returns>Hash SHA256 gerada</returns>
        public static string ToSha256(this string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            var hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            return hash.Aggregate(string.Empty, (current, x) => current + $"{x:x2}");
        }
        
        
        /// <summary>
        /// Retorna a representação numérica do texto
        /// </summary>
        /// <param name="input">Texto a ser convertido</param>
        /// <returns>Números contidos no texto</returns>
        public static string AsNumeric(this string input)
        {
            return new string(input.Where(char.IsDigit).ToArray());
        }
    }
}
