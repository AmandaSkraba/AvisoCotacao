using System.Timers;
using Cotacao.Modelos;

namespace Cotacao.Servicos
{
    internal class CotacaoServico
    {
        private System.Timers.Timer _timer;
        private readonly ConfiguracaoModelo _config;
        public decimal UltimaCotacao { get; set; } = 0;
        public string Ativo { get; init; }
        public decimal VlCompra { get; init; }
        public decimal VlVenda { get; init; }

        public CotacaoServico(ConfiguracaoModelo config)
        {
            _config = config;
            _timer = new System.Timers.Timer(new TimeSpan(0, config.Intervalo, 0)); // transforma intervalo em ms
            _timer.Elapsed += (sender, e) => Timer(sender, e);
            _timer.AutoReset = true;
        }
        public void IniciarMonitoramento()
        {
            VerificaCotacao();
            ReiniciarMonitoramento();
            Console.WriteLine("Iniciado o monitoramento de cotações.");

        }

        public void VerificaCotacao()
        {
            RetornoApiModelo retornoApi = ApiServico.ObterApi(Ativo, _config).Result;
            var cotacaoAtual = retornoApi.Retorno.FirstOrDefault();

            if (cotacaoAtual?.CotaAtual == UltimaCotacao)
            {
                Console.WriteLine($"A cotação atual é igual a última cotação realizada em {cotacaoAtual?.DataBusca.ToLocalTime()}. Verificado as {DateTime.Now}");
                return;
            }

            string mensagem = "";
            if (cotacaoAtual?.CotaAtual >= VlVenda)
            {
                mensagem = $"Está na hora de vender cotas.";
                Console.WriteLine($"{mensagem} ({cotacaoAtual.DataBusca.ToLocalTime()}, {cotacaoAtual.CotaAtual})");
                EmailServico.EnviarEmail(mensagem, cotacaoAtual, VlVenda);
                //EnviaEmail dizendo que está na hora de vender cotas.
            }
            else if (cotacaoAtual?.CotaAtual <= VlCompra)
            {
                mensagem = $"Está na hora de comprar cotas.";
                Console.WriteLine($"{mensagem} ({cotacaoAtual.DataBusca.ToLocalTime()}, {cotacaoAtual.CotaAtual})");
                EmailServico.EnviarEmail(mensagem, cotacaoAtual, VlCompra);
                //EnviaEmail dizendo que está na hora de comprar cotas.
            }
            else
            {
                mensagem = $"Sua cota se mantém entre os valores indicados para base de compra/venda.";
                Console.WriteLine($"{mensagem} ({cotacaoAtual?.DataBusca.ToLocalTime()}) \r\n" +
                    $"Valor para alerta de compra: {VlCompra}. \r\n" +
                    $"Valor para alerta de venda: {VlVenda}. \r\n" +
                    $"Valor cota atual: {cotacaoAtual?.CotaAtual}. \r\n");
            }

            UltimaCotacao = cotacaoAtual?.CotaAtual ?? 0;

        }

        private void Timer(object sender, ElapsedEventArgs e)
            => VerificaCotacao();

        public void PararMonitoramento()
           => _timer.Stop();

        public void ReiniciarMonitoramento()
          => _timer.Start();
    }
}
