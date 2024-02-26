namespace RinhaBackEnd2024.Models
{
    public record Extrato
    {
        public Saldo saldo { get; set; }
        public IEnumerable<Transacao> ultimas_transacoes { get; set; }
    }
    public record Saldo
    {
        public int total { get; set; }
        public int limite { get; set; }
        public DateTime data_extrato { get; set; }
    }
}
