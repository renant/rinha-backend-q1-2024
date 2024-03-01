
using RinhaBackEnd2024.Models;
using RinhaBackEnd2024.Persistence.Interfaces;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Dapper;

[module: DapperAot]

namespace RinhaBackEnd2024.Persistence.Repositories
{
    public class TransacaoRepository(IDbConnection connection) : ITransacaoRepository
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task<TransacaoResult> Adicionar(Transacao transacao)
        {
            return await connection.QueryFirstOrDefaultAsync<TransacaoResult>("SELECT * FROM adicionar_transacao(@cliente_id, @tipo, @valor, @descricao)", new
            {
                transacao.cliente_id,
                transacao.tipo,
                transacao.valor,
                transacao.descricao
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task<Extrato> BuscarExtrato(int id)
        {
            var extratoString = await connection.QueryFirstOrDefaultAsync<string>("SELECT * FROM extrato(@id)", new { id });
            return JsonSerializer.Deserialize(extratoString ?? "", SourceGenerationContext.Default.Extrato);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task Reset()
        {
            await connection.ExecuteAsync("UPDATE clientes set saldo = 0");
            await connection.ExecuteAsync("TRUNCATE table transacoes");
        }
    }
}
