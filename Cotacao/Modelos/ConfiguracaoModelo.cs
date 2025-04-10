namespace Cotacao.Modelos
{
    public class ConfiguracaoModelo
    {
        public string Email { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public int Intervalo { get; set; } = 1;
        public string TokenBrapi { get; set; }
    }
}
