using RinhaBackEnd2024.Models;

namespace RinhaBackEnd2024.Persistence.Interfaces
{
    public interface ITransacaoRepository
    {
        Task<Extrato> BuscarExtrato(int id);
        Task<TransacaoResult> Adicionar(Transacao transacao);
        Task Reset();
    }
}
