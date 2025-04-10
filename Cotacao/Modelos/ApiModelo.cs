using System.Text.Json.Serialization;

namespace Cotacao.Modelos
{
    internal class ApiModelo
    {
        [JsonPropertyName("longName")]
        public string Nome { get; set; }

        [JsonPropertyName("symbol")]
        public string Ativo { get; set; }

        [JsonPropertyName("regularMarketPrice")]
        public decimal CotaAtual { get; set; }

        [JsonPropertyName("regularMarketTime")]
        public DateTime DataBusca { get; set; }

        public string NomeAtivo => $"{Nome} ({Ativo})";
    }
}
