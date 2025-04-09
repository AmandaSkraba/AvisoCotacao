using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Cotacao
{
    class Program
    {
        private static CotacaoServico cotacao;

        static async Task Main(string[] args)
        {

            var config = ConfiguracaoServico.ObterConfiguracao();

            cotacao = new CotacaoServico(config)
            {
                Ativo = args[0],
                VlVenda = decimal.Parse(args[1], CultureInfo.InvariantCulture),
                VlCompra = decimal.Parse(args[2], CultureInfo.InvariantCulture),
            };

            cotacao.IniciarMonitoramento();
            ProcessarComandos();

        }

        static void ProcessarComandos()
        {
            bool rodando = true;
            string comando = null;

            Console.WriteLine("Menu: \r\n x => Encerrar o Monitoramento. \r\n p => Pausar o Monitoramento. \r\n r => Reiniciar o Monitoramento. \r\n a => Forçar nova pesquisa.");
            while (rodando)
            {
                comando = Console.ReadLine();
                switch (comando.Trim().ToLower())
                {
                    case "x":
                        cotacao.PararMonitoramento();
                        rodando = false;
                        Console.WriteLine("Monitoramento encerrado pelo usuário.");
                        break;
                    case "p":
                        cotacao.PararMonitoramento();
                        Console.WriteLine("Monitoramento pausado pelo usuário.");
                        break;
                    case "r":
                        cotacao.ReiniciarMonitoramento();
                        Console.WriteLine("Monitoramento reiniciado pelo usuário.");
                        break;
                    case "a":
                        cotacao.VerificaCotacao();
                        break;
                    case "e":
                        cotacao.PararMonitoramento();
                        ConfiguracaoServico.ResetarConfiguracao();
                        ConfiguracaoServico.ObterConfiguracao();
                        cotacao.UltimaCotacao = 0;
                        cotacao.VerificaCotacao();
                        break;
                }


            }
        }
    }
}