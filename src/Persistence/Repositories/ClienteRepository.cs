using Dapper;
using RinhaBackEnd2024.Models;
using RinhaBackEnd2024.Persistence.Interfaces;
using RinhaBackEnd2024.Persistence.UnitOfWork;

namespace RinhaBackEnd2024.Persistence.Repositories
{
    public class ClienteRepository(DbSession _session) : IClienteRepository
    {
        private const string queryBuscaCliente = "SELECT id, saldo, limite FROM Clientes WHERE id = @Id FOR UPDATE";
        private const string queryAtualizaSaldo = "UPDATE Clientes SET saldo = @Saldo WHERE id = @Id";

        public Task AtualizarSaldoCliente(Cliente cliente)
        {
            return _session.Connection.ExecuteAsync(queryAtualizaSaldo, new { cliente.Saldo, cliente.Id }, _session.Transaction);
        }

        public Task<Cliente?> BuscarCliente(int id)
        {
            return _session.Connection.QueryFirstOrDefaultAsync<Cliente>(queryBuscaCliente, new { Id = id }, _session.Transaction);
        }
    }
}
