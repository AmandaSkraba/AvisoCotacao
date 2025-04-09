using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cotacao
{
    public class ConfiguracaoServico
    {
        public static string CaminhoConfig => "config.json";

        public static ConfiguracaoModelo ObterConfiguracao()
        {
            if (File.Exists(CaminhoConfig))
            {
                string json = File.ReadAllText(CaminhoConfig);
                return JsonSerializer.Deserialize<ConfiguracaoModelo>(json);
            }

            var config = PrimeiraConfiguracao();

            try
            {
                SalvarConfiguracao(config);
                Console.WriteLine("Configuração salva com sucesso! Na próxima vez que entrar, seus dados já estrão salvos.");
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString());
            }

            return config;
        }

        public static ConfiguracaoModelo PrimeiraConfiguracao()
        {
            Console.WriteLine("Primeira vez usando? Vamos configurar seu ambiente!");

            ConfiguracaoModelo config = new ConfiguracaoModelo();

            Console.WriteLine("Primeiro, digite o e-mail que deseja ser notificado: ");
            config.Email = Console.ReadLine();

            Console.WriteLine("Agora, digite o Servidor SMTP do email: ");
            config.Host = Console.ReadLine();

            Console.WriteLine("Em seguida, digite a porta SMTP: ");
            config.Port = int.Parse(Console.ReadLine());

            Console.WriteLine("Agora, digite o Usuário (email que enviará o aviso): ");
            config.Usuario = Console.ReadLine();

            Console.WriteLine("Digite também a senha do email (fica tranquilo(a), ela vai ser criptografada): ");
            config.Senha = Console.ReadLine();

            Console.WriteLine("Digite o intervalo, em minutos, para que eu realize a verificação de cotação: ");
            config.Intervalo = int.Parse(Console.ReadLine());

            Console.WriteLine("Para finalizar, vou precisar do seu token da API FinnHub, é gratuito e você pode obtê-lo em https://finnhub.io/: ");
            config.TokenBrapi = Console.ReadLine();

            return config;
        }

        public static void SalvarConfiguracao(ConfiguracaoModelo novaConfiguracao)
        {
            var json = JsonSerializer.Serialize(novaConfiguracao);
            File.WriteAllText(CaminhoConfig, json);
        }

        public static void ResetarConfiguracao()
            => File.Delete(CaminhoConfig);
        
    }
}
