using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Cotacao
{
    class Program
    {
        private static CotacaoServico? cotacao;

        static void Main(string[] args)
        {
            var parametros = RecuperarArgumentos(args);

            if (!parametros.encerrar)
            {
                var config = ConfiguracaoServico.ObterConfiguracao();

                cotacao = new CotacaoServico(config)
                {
                    Ativo = parametros.ativo,
                    VlVenda = parametros.vlVenda,
                    VlCompra = parametros.vlCompra
                };

                cotacao.IniciarMonitoramento();
                ProcessarComandos();
            }           

        }

        static void ProcessarComandos()
        {
            bool rodando = true;
            string comando = "";

            Console.WriteLine("Menu: \r\n x => Encerrar o Monitoramento. \r\n p => Pausar o Monitoramento. \r\n r => Reiniciar o Monitoramento. \r\n a => Forçar nova pesquisa. \r\n c => Alterar dados de configuração.");
            while (rodando)
            {
                comando = Console.ReadLine() ?? "";
                switch (comando?.Trim().ToLower())
                {
                    case "x":
                        cotacao?.PararMonitoramento();
                        rodando = false;
                        Console.WriteLine("Monitoramento encerrado pelo usuário.");
                        break;
                    case "p":
                        cotacao?.PararMonitoramento();
                        Console.WriteLine("Monitoramento pausado pelo usuário.");
                        break;
                    case "r":
                        cotacao?.ReiniciarMonitoramento();
                        Console.Clear();
                        Console.WriteLine("Monitoramento reiniciado pelo usuário.");
                        Console.WriteLine("Menu: \r\n x => Encerrar o Monitoramento. \r\n p => Pausar o Monitoramento. \r\n r => Reiniciar o Monitoramento. \r\n a => Forçar nova pesquisa. \r\n c => Alterar dados de configuração.");
                        break;
                    case "a":
                        cotacao?.VerificaCotacao();
                        break;
                    case "c":
                        cotacao?.PararMonitoramento();
                        ConfiguracaoServico.ResetarConfiguracao();
                        ConfiguracaoServico.ObterConfiguracao();
                        cotacao.UltimaCotacao = 0;
                        cotacao?.VerificaCotacao();
                        break;
                }


            }
        }

        static (string ativo, decimal vlVenda, decimal vlCompra, bool encerrar) RecuperarArgumentos(string[] args)
        {
            if (args.Length == 3)
                return (args[0],
                        decimal.Parse(args[1].Replace(",", "."), CultureInfo.InvariantCulture),
                        decimal.Parse(args[2].Replace(",", "."), CultureInfo.InvariantCulture),
                        false);

            string ativo = "";
            decimal vlVenda = 0;
            decimal vlCompra = 0;
            bool encerrar = false;
            
            solicitar:
            try
            {

                Console.Clear();
                Console.WriteLine("Você não adicionou os parâmetros de monitoramento. Então vamos fazer isso: ");
                Console.WriteLine("Digite o nome do Ativo que você deseja monitorar: ");
                var ativoDigitado = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(ativoDigitado)) Console.WriteLine($"Usando o último valor digitado: {ativo}");
                ativo = string.IsNullOrWhiteSpace(ativoDigitado) ? ativo : ativoDigitado;

                Console.WriteLine("Digite o valor base para Venda: ");
                var vlVendaDigitado = Console.ReadLine();
                vlVenda = string.IsNullOrWhiteSpace(vlVendaDigitado) ? vlVenda : decimal.Parse(vlVendaDigitado.Replace(",", "."), CultureInfo.InvariantCulture);

                Console.WriteLine("Digite o valor base para Compra: ");
                var vlCompraDigitado = Console.ReadLine();
                vlCompra = string.IsNullOrWhiteSpace(vlCompraDigitado) ? vlCompra : decimal.Parse(vlCompraDigitado.Replace(",", "."), CultureInfo.InvariantCulture);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Não foi possível adicionar os parâmetros devido a esse erro: {ex}");

                Console.WriteLine($"Para tentar novamente, tecle 's' e confirme.");
                if ((new string[] { "s", "y", "sim", "yes" })
                    .Contains(Console.ReadLine().ToLower()))
                    goto solicitar;

                Console.WriteLine($"Encerrando...");
                encerrar = true;
            }
            return (ativo, vlVenda, vlCompra, encerrar);

        }
    }
}