using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cotacao
{
    internal class ApiServico
    {
        public static async Task<RetornoApiModelo> ObterApi(string ativo, ConfiguracaoModelo config)
        {
            HttpClient httpClient = new HttpClient();
            string url = $"https://brapi.dev/api/quote/{ativo}?token={config.TokenBrapi}";


            HttpResponseMessage res = await httpClient.GetAsync(url);

            if (res == null) throw new Exception($"Não foi possível recuperar a cotação atual.");

            if (res.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<RetornoApiModelo>(await res?.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


            //await res.Content.ReadFromJsonAsync<RetornoApiModelo>(new JsonSerializerOptions
            //                                           { 
            //                                               PropertyNameCaseInsensitive = true
            //                                           });
            throw new Exception($"Não foi possível recuperar a cotação atual: {res?.RequestMessage?.Content?.ReadAsStringAsync().Result}");

        }
    }
}
