using Dapper;
using Npgsql;
using RinhaBackEnd2024.Models;
using RinhaBackEnd2024.Persistence.Interfaces;
using System.Data;
using System.Text.Json;

namespace RinhaBackEnd2024.Persistence.Repositories
{
    public class TransacaoRepository(IDbConnection _connection) : ITransacaoRepository
    {
        public async Task<Extrato> BuscarExtrato(int id)
        {
            var result = await _connection.QueryFirstOrDefaultAsync<string>("SELECT * FROM extrato(@id)", new { id = id });

            var extrato = JsonSerializer.Deserialize<Extrato>(result ?? "");

            return extrato;
        }

        public async Task<TransacaoResult> Adicionar(Transacao transacao)
        {
            try
            {
                var atualizarSaldo = await _connection.QuerySingleOrDefaultAsync<TransacaoResult>("SELECT * FROM adicionar_transacao(@cliente_id, @tipo, @valor, @descricao)", new { transacao.cliente_id, transacao.tipo, transacao.valor, transacao.descricao });

                return atualizarSaldo;
            }
            catch (PostgresException ex)
            {
                return new TransacaoResult { validation_error = true };
            }
        }
    }
}
