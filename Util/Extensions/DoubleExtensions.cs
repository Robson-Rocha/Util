using System;
using System.Collections.Generic;

namespace RobsonROX.Util.Extensions
{
    /// <summary>
    /// Métodos de extensão para double
    /// </summary>
    public static class DoubleExtensions
    {
        private static readonly string[, ,] Names =
        {
            {
                {"primeiro", "segundo", "terceiro", "quarto", "quinto", "sexto", "sétimo", "oitavo", "nono"},
                {"décimo", "vigésimo", "trigésimo", "quadragésimo", "quinquagésimo", "sexagésimo", "septuagésimo", "octagésimo", "nonagésimo"},
                {"centésimo", "ducentésimo", "trecentésimo", "quadrigentésimo", "quingentésimo", "sexcentésimo", "septigentésimo", "octigentésimo", "nongentésimo"}
            },
            {
                {"primeira", "segunda", "terceira", "quarta", "quinta", "sexta", "sétima", "oitava", "nona"},
                {"décima", "vigésima", "trigésima", "quadragésima", "quinquagésima", "sexagésima", "septuagésima", "octagésima", "nonagésima"},
                {"centésima", "ducentésima", "trecentésima", "quadrigentésima", "quingentésima", "sexcentésima", "septigentésima", "octigentésima", "nongentésima"}
            }
        };

        private static string ToOrdinal(double number, bool feminine, bool first = true)
        {
            if (!first && Math.Abs(number - 1) < double.Epsilon)
                return null;
            int g = feminine ? 1 : 0;
            var ordinal = new List<string>();
            double digits = Math.Floor(Math.Log10(number) + 1);
            double previous = 0;
            for (int i = 1; i <= digits; i++)
            {
                double current = number % Math.Pow(10, i);
                double order = Math.Pow(10, i - 1);
                double digit = (current - previous) / order;
                string name = null;
                if (i <= 3)
                {
                    if (Math.Abs(digit) > double.Epsilon)
                        name = Names[g, i - 1, (int)digit - 1];
                }
                else
                {
                    ordinal.Insert(0, feminine ? "milésima" : "milésimo");
                    name = ToOrdinal((number - previous) / order, feminine, false);
                }

                if (name != null)
                    ordinal.Insert(0, name);

                if (i > 3)
                    break;

                previous = current;
            }
            return string.Join(" ", ordinal);
        }

        /// <summary>
        /// Retorna a representação ordinal por extenso do número fornecido
        /// </summary>
        /// <param name="number">Número a ser representado por extenso</param>
        /// <returns>TRepresentação ordinal por extenso</returns>
        public static string ToOrdinal(this double number)
        {
            return ToOrdinal(number, false);
        }

        /// <summary>
        /// Retorna a representação ordinal, no feminino, por extenso do número fornecido
        /// </summary>
        /// <param name="number">Número a ser representado por extenso</param>
        /// <returns>TRepresentação ordinal no feminino por extenso</returns>
        public static string ToFeminineOrdinal(this double number)
        {
            return ToOrdinal(number, true);
        }

        /// <summary>
        /// Retorna a representação ordinal, por extenso do número fornecido
        /// </summary>
        /// <param name="number">Número a ser representado por extenso</param>
        /// <returns>TRepresentação ordinal por extenso</returns>
        public static string ToOrdinal(this int number)
        {
            return ToOrdinal(number, false);
        }

        /// <summary>
        /// Retorna a representação ordinal, no feminino, por extenso do número fornecido
        /// </summary>
        /// <param name="number">Número a ser representado por extenso</param>
        /// <returns>TRepresentação ordinal no feminino por extenso</returns>
        public static string ToFeminineOrdinal(this int number)
        {
            return ToOrdinal(number, true);
        }
    }
}
