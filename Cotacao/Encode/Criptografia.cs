using System.Text;

namespace Cotacao.Encode
{
    public class Criptografia
    {
        public static string CodificarBase64(string texto)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(texto);
            return Convert.ToBase64String(bytes);
        }

        public static string DecodificarBase64(string textoBase64)
        {
            byte[] bytes = Convert.FromBase64String(textoBase64);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
