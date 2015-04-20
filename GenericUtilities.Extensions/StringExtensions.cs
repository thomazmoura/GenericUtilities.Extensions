using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace GenericUtilities.Extensions
{
    /// <summary> Conjunto de métodos de extensão da classe String para facilitar o uso de strings </summary>
    public static class StringExtensions
    {
        //Essa classe possui códigos adaptados da internet.
        //Para a fonte inicial de alguns desses métodos acesse http://stackoverflow.com/questions/19523913/remove-html-tags-from-string-including-nbsp-in-c-sharp .
        
        #region Métodos para remoção de tags

        /// <summary> Regex para identificar tags html ou xml.</summary>
        private static readonly Regex tags = new Regex(@"<[^>]+?>", RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary> Remove quaisquer tags (HTML, XML, etc.) encontradas no texto original.
        /// <para>Esse método não elimina o conteúdo interno da tag.</para> </summary>
        /// <param name="source"> String original. </param>
        /// <returns> String contendo o texto original desprovido de tags. </returns>
        public static string RemoverTags(this string source)
        {
            //Caso o método seja chamado em uma string vazia ou nula, retorne uma string vazia
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            return tags.Replace(source, string.Empty);
        }

        /// <summary> Elimina todas as tags HTML encontradas na string, bem como o conteúdo interno de tags de comentário, script ou style,
        ///  mantendo apenas texto corrido separado por espaços simples. </summary>
        /// <param name="html"> Texto original cujo HTML será removido. </param>
        /// <returns> String contendo apenas o texto original, ignorando quaisquer tags ou conteúdo específico de HTML (como comentários, scripts ou css). </returns>
        public static String RemoverHtml(this String html)
        {
            //Caso o método seja chamado em uma string vazia ou nula, retorne uma string vazia
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            html = HttpUtility.UrlDecode(html);
            html = HttpUtility.HtmlDecode(html);

            html = RemoverTag(html, "<!--", "-->");
            html = RemoverTag(html, "<script", "</script>");
            html = RemoverTag(html, "<style", "</style>");

            //Utiliza os Regex para substituir qualquer correspondência (tags) encontrada no texto por espaço
            html = tags.Replace(html, " ");
            html = ManterEspacoSimples(html);

            return html;
        }

        /// <summary> Remove uma tag específica e todo seu conteúdo interno. </summary>
        /// <param name="html"> Texto original do qual será extraída a tag. </param>
        /// <param name="inicioTag"> Demarcação do início da Tag. </param>
        /// <param name="finalTag"> Demarcação do final da Tag. </param>
        /// <returns> String contendo o texto original menos qualquer ocorrência da tag informada e seu conteúdo interno. </returns>
        public static String RemoverTag(this String html, String inicioTag, String finalTag)
        {
            //Caso o método seja chamado em uma string vazia ou nula, retorne uma string vazia
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            Boolean repetir;
            do
            {
                repetir = false; //Variável para controle da repetição
                //Procura pelo início da tag no texto
                Int32 posicaoInicialTag = html.IndexOf(inicioTag, 0, StringComparison.CurrentCultureIgnoreCase);
                //Caso não seja encontrado o fim da tag, sai do loop.
                if (posicaoInicialTag < 0)
                    break;
                //Procura pelo fim da tag no texto
                Int32 posicaoFinalTag = html.IndexOf(finalTag, posicaoInicialTag + 1, StringComparison.CurrentCultureIgnoreCase);
                //Caso não seja encontrado o fim da tag, sai do loop.
                if (posicaoFinalTag <= posicaoInicialTag)
                    break;
                //Remove todo o trecho entre o início e o final da tag
                html = html.Remove(posicaoInicialTag, posicaoFinalTag - posicaoInicialTag + finalTag.Length);
                //Executa novamente a verificação para garantir que não haja mais nenhuma tag
                repetir = true;
            } while (repetir);
            return html;
        }
        #endregion
        
        #region Métodos para formatação ou limpeza de texto
        /// <summary> Substitui toda quebra de linha ou espaço longo por espaços simples. </summary>
        /// <param name="textoOriginal"> O texto cujos espaços serão removidos. </param>
        /// <returns> string contendo o texto original sem quebras de linha e apenas com espaços simples entre as palavras. </returns>
        public static string ManterEspacoSimples(this string textoOriginal)
        {
            //Caso o método seja chamado em uma string vazia ou nula, retorne uma string vazia
            if (string.IsNullOrEmpty(textoOriginal))
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            Boolean inBlanks = false; //Define se já foi inserido um espaço antes do caracter atual
            foreach (Char c in textoOriginal)
            {
                //Se o caracter for um espaço vazio (espaços, tabulações, quebras de linha, etc.)
                if (Char.IsWhiteSpace(c))
                {
                    //Apenas se não já tiver sido inserido um espaço anterior, insira um espaço vazio
                    if (!inBlanks)
                    {
                        inBlanks = true;
                        sb.Append(' ');
                    }
                }
                else
                {
                    inBlanks = false;
                    sb.Append(c);
                }
            }
            return sb.ToString().Trim();
        }
        #endregion
    }
}
