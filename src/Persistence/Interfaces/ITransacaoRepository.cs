using RinhaBackEnd2024.Models;

namespace RinhaBackEnd2024.Persistence.Interfaces
{
    public interface ITransacaoRepository
    {
        Task<IEnumerable<Transacao>> BuscarUltimasTransacoes(int id);
        Task InserirTransacao(Transacao transacao);
    }
}
