using System.Text.Json;
using Cotacao.Encode;
using Cotacao.Modelos;

namespace Cotacao.Servicos
{
    public class ConfiguracaoServico
    {
        public static string CaminhoConfig => "config.json";

        public static ConfiguracaoModelo ObterConfiguracao()
        {
            try
            {
                if (File.Exists(CaminhoConfig))
                {
                    string json = Criptografia.DecodificarBase64(File.ReadAllText(CaminhoConfig));
                    return JsonSerializer.Deserialize<ConfiguracaoModelo>(json);
                }

                var config = PrimeiraConfiguracao();
                SalvarConfiguracao(config);
                Console.WriteLine("Configuração salva com sucesso! Na próxima vez que entrar, seus dados já estrão salvos.");
                return config;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

        }

        public static ConfiguracaoModelo PrimeiraConfiguracao()
        {
            Console.WriteLine("Primeira vez usando? Vamos configurar seu ambiente!");

            ConfiguracaoModelo config = new ConfiguracaoModelo();

            Console.WriteLine("Primeiro, digite o e-mail que deseja receber os avisos: ");
            config.Email = Console.ReadLine();

            Console.WriteLine("Agora, digite o Servidor SMTP do email: ");
            config.Host = Console.ReadLine();

            Console.WriteLine("Em seguida, digite a porta SMTP: ");
            config.Port = int.Parse(Console.ReadLine());

            Console.WriteLine("Agora, digite o Usuário (email que enviará o aviso): ");
            config.Usuario = Console.ReadLine();

            Console.WriteLine("Digite também a senha do email (Se seu email for GMAIL, você precisará de uma senha de app, crie seguindo o manual https://support.google.com/accounts/answer/185833?hl=pt-BR e insira aqui): ");
            config.Senha = Console.ReadLine();

            Console.WriteLine("Digite o intervalo, em minutos, para que eu realize a verificação de cotação: ");
            config.Intervalo = int.Parse(Console.ReadLine());

            Console.WriteLine("Para finalizar, vou precisar do seu token da API Brapi, é gratuito e você pode obtê-lo em https://brapi.dev/, basta se cadastrar: ");
            config.TokenBrapi = Console.ReadLine();


            return config;
        }

        public static void SalvarConfiguracao(ConfiguracaoModelo novaConfiguracao)
        {
            ResetarConfiguracao();
            var json = Criptografia.CodificarBase64(JsonSerializer.Serialize(novaConfiguracao));
            File.WriteAllText(CaminhoConfig, json);
        }

        public static void ResetarConfiguracao()
            => File.Delete(CaminhoConfig);

    }
}
