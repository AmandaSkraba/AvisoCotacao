using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cotacao
{
    internal class RetornoApiModelo
    {
        [JsonPropertyName("results")]
        public List<ApiModelo> Retorno { get; set; }
    }
}
