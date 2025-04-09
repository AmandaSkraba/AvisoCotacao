using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cotacao
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
