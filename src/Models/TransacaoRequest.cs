namespace RinhaBackEnd2024.Models
{
    public record TransacaoRequest
    {
        public int valor { get; set; }
        public string tipo { get; set; }
        public string descricao { get; set; }
    }
}
