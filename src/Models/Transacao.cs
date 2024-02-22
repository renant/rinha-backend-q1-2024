using System.Text.Json.Serialization;

namespace RinhaBackEnd2024.Models
{
    public class Transacao
    {
        public int valor { get; set; }
        public char tipo { get; set; }
        public string descricao { get; set; }
        public DateTime realizada_em { get; set; }


        [JsonIgnore]
        public int valorNormalizado => valor * (tipo == 'd' ? -1 : 1);

        [JsonIgnore]
        public int cliente_id { get; set; }

        public Transacao(int valor, char tipo, string descricao, int cliente_id)
        {
            realizada_em = DateTime.Now;
            this.valor = valor;
            this.tipo = tipo;
            this.descricao = descricao;
            this.cliente_id = cliente_id;
        }

        public Transacao()
        {
        }
    }
}
