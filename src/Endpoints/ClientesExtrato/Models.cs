using RinhaBackEnd2024.Models;

namespace Endpoints.ClientesExtrato
{
    public class Request
    {
        public int id { get; set; }
    }


    public class Response
    {
        public Saldo saldo { get; set; }
        public IEnumerable<Transacao> ultimas_transacoes { get; set; }
    }
    public class Saldo
    {
        public int total { get; set; }
        public int limite { get; set; }
        public DateTime data_extrato => DateTime.Now;
    }
}
