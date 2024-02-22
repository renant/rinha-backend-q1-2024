using RinhaBackEnd2024.Models;

namespace RinhaBackEnd2024.Persistence.Interfaces
{
    public interface IClienteRepository
    {
        Task AtualizarSaldoCliente(Cliente cliente);
        Task<Cliente?> BuscarCliente(int id);
    }
}
