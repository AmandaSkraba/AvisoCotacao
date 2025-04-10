using System.Globalization;
using System.Net;
using System.Net.Mail;
using Cotacao.Modelos;

namespace Cotacao.Servicos
{
    internal class EmailServico
    {
        public static void EnviarEmail(string mensagemCota, ApiModelo ativo, decimal vlIndicado)
        {
            ConfiguracaoModelo config = ConfiguracaoServico.ObterConfiguracao();

            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress(config.Email, "Amanda Skraba - Aviso de Cotações", System.Text.Encoding.UTF8);
            mensagem.To.Add(config.Email);
            mensagem.Subject = $"Informação sobre Cotas: {ativo.Ativo}";
            mensagem.IsBodyHtml = true;
            mensagem.Body = $"<h3>Olá, temos notícias sobre sua cotação {ativo.Ativo}:</h3>" + "<strong>Mensagem: </strong>" + mensagemCota + "<br>" + "<strong>Valor indicado: </strong> R$ " + vlIndicado + "<br>" + "<strong>Valor da Cotação atual:</strong> R$ " + ativo.CotaAtual.ToString("F2", CultureInfo.InvariantCulture).Replace(".", ",") + "<br>" + "<strong>Data da Cotação atual: </strong>" + ativo.DataBusca.ToLocalTime();

            ProcessarEnvio(mensagem, config);
        }

        private static void ProcessarEnvio(MailMessage message, ConfiguracaoModelo config)
        {
            try
            {
                var smtp = new SmtpClient();
                smtp.EnableSsl = true;
                smtp.Host = config.Host;
                smtp.Port = config.Port;
                smtp.Timeout = 10000;
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                //email de envio, senha = token gerado no gmail (Senha de app)
                smtp.Credentials = new NetworkCredential(config.Usuario, config.Senha);
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao enviar a mensagem: " + ex.Message, ex);
            }
        }
    }
}