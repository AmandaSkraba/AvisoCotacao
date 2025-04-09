using System.Net;
using System.Net.Mail;

namespace Cotacao
{
    internal class EmailServico
    {
        public static void EnviarEmail(string mensagemCota, ApiModelo ativo, decimal vlIndicado)
        {
            ConfiguracaoModelo arquivo = ConfiguracaoServico.ObterConfiguracao();

            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress(arquivo.Email, "Amanda Skraba - Aviso de Cotações", System.Text.Encoding.UTF8);
            mensagem.To.Add(arquivo.Email);
            mensagem.Subject = $"Informação sobre Cotas: {ativo.Ativo}";
            mensagem.IsBodyHtml = true;
            mensagem.Body = $"<h3>Olá, temos notícias sobre sua cotação {ativo.Ativo}:</h3>" + "<strong>Mensagem: </strong>" + mensagemCota + "<br>" + "<strong>Valor indicado: </strong> R$ " + vlIndicado + "<br>" + "<strong>Valor da Cotação atual:</strong> R$ " + ativo.CotaAtual + "<br>" + "<strong>Data da Cotação atual: </strong>" + ativo.DataBusca.ToLocalTime();

            ProcessarEnvio(mensagem, arquivo);
        }

        private static void ProcessarEnvio(MailMessage message, ConfiguracaoModelo arquivo)
        {
            try
            {
                var smtp = new SmtpClient();
                smtp.EnableSsl = true; // true
                smtp.Host = arquivo.Host; //smtp
                smtp.Port = arquivo.Port;//porta
                smtp.Timeout = 10000; //6000
                smtp.UseDefaultCredentials = false; //false
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; //0

                //email de envio, senha = token gerado no gmail (Senha de app)
                smtp.Credentials = new NetworkCredential(arquivo.Email, arquivo.Senha);
                smtp.Send(message);
            }
            catch (Exception ex)
            {               
                throw new Exception("Falha ao enviar a mensagem: " + ex.Message, ex);
            }
        }
    }
}