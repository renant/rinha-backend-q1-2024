using Dapper;
using RinhaBackEnd2024.Models;
using RinhaBackEnd2024.Persistence.Interfaces;
using RinhaBackEnd2024.Persistence.UnitOfWork;

namespace RinhaBackEnd2024.Persistence.Repositories
{
    public class TransacaoRepository(DbSession _session) : ITransacaoRepository
    {
        private const string queryBuscarUltimas10Transacoes = "SELECT valor, tipo, descricao, realizada_em FROM Transacoes WHERE cliente_id = @Id ORDER BY realizada_em DESC LIMIT 10";
        private const string queryInserirTransacao = "INSERT INTO transacoes (valor, tipo, descricao, realizada_em, cliente_id) VALUES (@valor, @tipo, @descricao, @realizada_em, @cliente_id)";

        public Task<IEnumerable<Transacao>> BuscarUltimasTransacoes(int id)
        {
            return _session.Connection.QueryAsync<Transacao>(queryBuscarUltimas10Transacoes, new { Id = id }, _session.Transaction);
        }

        public Task InserirTransacao(Transacao transacao)
        {
            return _session.Connection.ExecuteAsync(
                queryInserirTransacao,
                new
                {
                    transacao.valor,
                    transacao.tipo,
                    transacao.descricao,
                    transacao.realizada_em,
                    transacao.cliente_id
                },
                _session.Transaction
                );
        }
    }
}
