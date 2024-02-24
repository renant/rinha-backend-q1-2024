using System.Text.Json.Serialization;

namespace RinhaBackEnd2024.Models
{
    public class TransacaoResult
    {
        [JsonPropertyName("saldo")]
        public int novo_saldo { get; set; }

        [JsonPropertyName("limite")]
        public int novo_limite { get; set; }

        [JsonIgnore]
        public bool validation_error { get; set; }
    }
}
