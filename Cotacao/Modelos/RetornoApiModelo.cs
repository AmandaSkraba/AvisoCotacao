using System.Text.Json.Serialization;

namespace Cotacao.Modelos
{
    internal class RetornoApiModelo
    {
        [JsonPropertyName("results")]
        public List<ApiModelo> Retorno { get; set; }
    }
}
