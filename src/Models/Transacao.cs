using System.Text.Json.Serialization;

namespace RinhaBackEnd2024.Models
{
    public record Transacao
    {
        public int valor { get; set; }
        public string tipo { get; set; }
        public string descricao { get; set; }
        public DateTime realizada_em { get; set; }


        [JsonIgnore]
        public int valorNormalizado => valor * (tipo == "d" ? -1 : 1);

        [JsonIgnore]
        public int cliente_id { get; set; }

        public Transacao(int valor, string tipo, string descricao, int cliente_id)
        {
            realizada_em = DateTime.Now;
            this.valor = valor;
            this.tipo = tipo;
            this.descricao = descricao;
            this.cliente_id = cliente_id;
        }

        public bool IsValid()
        {
            return
                valor > 0 &&
                (tipo == "d" || tipo == "c")
                && !string.IsNullOrEmpty(descricao ?? "")
                && (descricao ?? "").Length <= 10;
        }
    }
}
