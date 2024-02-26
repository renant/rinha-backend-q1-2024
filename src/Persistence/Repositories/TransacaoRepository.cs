using Npgsql;
using RinhaBackEnd2024.Models;
using RinhaBackEnd2024.Persistence.Interfaces;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace RinhaBackEnd2024.Persistence.Repositories
{
    public class TransacaoRepository(NpgsqlDataSource dataSource) : ITransacaoRepository
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task<TransacaoResult> Adicionar(Transacao transacao)
        {
            await using var conn = await dataSource.OpenConnectionAsync();

            try
            {
                var retries = 0;
                while (retries < 10)
                {
                    try
                    {
                        using var command = new NpgsqlCommand("SELECT * FROM adicionar_transacao(@cliente_id, @tipo, @valor, @descricao)", conn);
                        command.Parameters.AddWithValue("cliente_id", transacao.cliente_id);
                        command.Parameters.AddWithValue("tipo", transacao.tipo);
                        command.Parameters.AddWithValue("valor", transacao.valor);
                        command.Parameters.AddWithValue("descricao", transacao.descricao);
                        using var result = await command.ExecuteReaderAsync();
                        await result.ReadAsync();

                        var transacaoResult = new TransacaoResult
                        {
                            novo_saldo = result.GetInt32(0),
                            novo_limite = result.GetInt32(1),
                            validation_error = result.GetBoolean(2)
                        };

                        return transacaoResult;
                    }
                    catch (Exception ex)
                    {
                        retries++;
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                }

                return new TransacaoResult
                {
                    validation_error = true
                };
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task<Extrato> BuscarExtrato(int id)
        {
            await using var conn = await dataSource.OpenConnectionAsync();

            try
            {
                using var command = new NpgsqlCommand("SELECT * FROM extrato(@id)", conn);
                command.Parameters.AddWithValue("id", id);
                using var result = await command.ExecuteReaderAsync();
                await result.ReadAsync();

                var extrato = JsonSerializer.Deserialize<Extrato>(result.GetString(0) ?? "");
                return extrato;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task Reset()
        {
            await using var conn = await dataSource.OpenConnectionAsync();

            try
            {
                await using var cmd = conn.CreateBatch();
                cmd.BatchCommands.Add(new NpgsqlBatchCommand("UPDATE clientes set saldo = 0"));
                cmd.BatchCommands.Add(new NpgsqlBatchCommand("TRUNCATE table transacoes"));
                using var reader = await cmd.ExecuteReaderAsync();
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
    }
}
