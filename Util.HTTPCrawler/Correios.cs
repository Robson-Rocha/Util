using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace RobsonROX.Util.HTMLParsers
{
    /// <summary>
    /// Possui métodos para interação com o site dos correios para busca de logradouros
    /// </summary>
    public static class Correios
    {
        //TODO: [RobsonROX] Prever CEPs especiais, de cidade e etc.
        /// <summary>
        /// Consulta no site dos Correios pelo CEP ou parte do endereço informado
        /// </summary>
        /// <param name="cep">CEP ou parte do endereço a ser procurado</param>
        /// <returns>Lista de endereços obtidos, ou null caso nenhum endereço seja encontrado</returns>
        /// <exception cref="HtmlWebException">Lançada caso ocorra um erro no processamento da página de resultados</exception>
        /// <exception cref="Exception">Lançada caso um erro não previsto ocorra.</exception>
        public static async Task<List<Dictionary<string, string>>> BuscaEnderecos(string cep)
        {
            return await BuscaEnderecos(cep, "buscarCep", null, null);
        }

        private static async Task<List<Dictionary<string, string>>> BuscaEnderecos(
            string cep, 
            string metodo, 
            string numPagina,
            string regTotal,
            CookieContainer cookies = null)
        {
            try
            {
                return await Task.Run(async () =>
                {

                    //Efetua a consulta no site móvel dos correios
                    cookies = cookies ?? new CookieContainer();
                    string response = HttpRequest.Request("http://m.correios.com.br/movel/buscaCepConfirma.do",
                        $"metodo={metodo}&cepEntrada={cep}&tipoCep=&cepTemp=" + (numPagina != null ?
                            $"&numPagina={numPagina}&regTotal={regTotal}"
                            : ""),
                                                          HttpRequest.HttpMethods.Post);

                    //Carrega o HTML obtido no parser
                    var html = new HtmlDocument { OptionFixNestedTags = true };
                    html.LoadHtml(response);
                    if (html.ParseErrors != null && html.ParseErrors.Any())
                    {
                        var ex = new HtmlWebException("O HTML obtido é inválido");
                        ex.Data.Add("cep", cep);
                        ex.Data.Add("errors", html.ParseErrors);
                        throw ex;
                    }
                    if (html.DocumentNode == null)
                    {
                        var ex = new HtmlWebException("O HTML obtido não possui um nó de documento válido.");
                        ex.Data.Add("cep", cep);
                        throw ex;
                    }

                    //Verifica se foi retornado um nó de erro
                    if (html.DocumentNode.SelectSingleNode(@"//div[@class='erro']") != null) return null;

                    //Obtém os nós de resultado
                    HtmlNodeCollection nodesBrancos = html.DocumentNode.SelectNodes(@"//div[@class='caixacampobranco']");
                    HtmlNodeCollection nodesAzuis = html.DocumentNode.SelectNodes(@"//div[@class='caixacampoazul']");
                    var nodes = new List<HtmlNode>();
                    if (nodesBrancos != null)
                        nodes = nodes.Concat(nodesBrancos).ToList();
                    if (nodesAzuis != null)
                        nodes = nodes.Concat(nodesAzuis).ToList();

                    //Verifica se foi obtido algum resultado
                    if (!nodes.Any())
                    {
                        var ex = new HtmlWebException("O HTML obtido não está na estrutura correta. Talvez o site dos correios tenha sofrido modificações.");
                        ex.Data.Add("cep", cep);
                        throw ex;
                    }

                    //Processa os resultados obtidos
                    var ret = (from node in nodes
                               select node.SelectNodes(@"//span[@class='respostadestaque']")
                                   into detailNodes
                                   where detailNodes.Any()
                                   let cidadeUf = detailNodes[2].InnerText.Split('/')
                                   select new Dictionary<string, string> {
                                {"logradouro", detailNodes[0].InnerText.Trim()}, 
                                {"bairro", detailNodes[1].InnerText.Trim()}, 
                                {"cidade", cidadeUf[0].Trim().Trim()}, 
                                {"uf", cidadeUf[1].Trim().Trim()},
                            }).ToList();

                    //Verifica se há mais páginas de retorno, e se houver, as verifica
                    if (html.DocumentNode.SelectSingleNode(@"//input[@type='button' and @class='botao' and @value='Próximo']") != null)
                    {
                        string valNumPagina = html.DocumentNode.SelectSingleNode(@"//input[@type='hidden' and @name='numPagina']").Attributes["value"].Value;
                        string valRegTotal = html.DocumentNode.SelectSingleNode(@"//input[@type='hidden' and @name='regTotal']").Attributes["value"].Value;
                        var enderecos = await BuscaEnderecos(cep, "proximo", valNumPagina, valRegTotal, cookies);
                        ret.AddRange(enderecos);
                    }

                    //Retorna os resultados obtidos
                    return ret;
                });
            }
            catch (HtmlWebException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception("Ocorreu um erro inesperado. Verifique a InnerException para mais detalhes.", e);
            }
        }
    }
}
