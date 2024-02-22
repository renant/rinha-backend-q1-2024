using RinhaBackEnd2024.Models;
using RinhaBackEnd2024.Persistence.Interfaces;

namespace Endpoints
{
    public class Endpoint(IClienteRepository _clienteRepository, ITransacaoRepository _transacaoRepository, IUnitOfWork _unitOfWork) : Endpoint<Request, Response, Mapper>
    {
        public override void Configure()
        {
            Post("/clientes/{id}/transacoes");
            AllowAnonymous();
            DontThrowIfValidationFails();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                if (ValidationFailed)
                {
                    ThrowError(ValidationFailures.FirstOrDefault().ErrorMessage, 422);
                    return;
                }

                if (req.id < 1 || req.id > 5)
                    ThrowError("Cliente não encontrado", 404);

                var transacao = new Transacao
                        (
                            req.transacao.valor,
                            req.transacao.tipo,
                            req.transacao.descricao,
                            req.id
                        );

                var cliente = await _clienteRepository.BuscarCliente(req.id);


                if (cliente == null)
                {
                    await SendNotFoundAsync();
                    return;
                }

                cliente.Saldo += transacao.valorNormalizado;

                if (cliente.Saldo < (cliente.Limite * -1))
                    ThrowError("Limite de crédito excedido", 422);

                await _clienteRepository.AtualizarSaldoCliente(cliente);
                await _transacaoRepository.InserirTransacao(transacao);

                _unitOfWork.Commit();

                await SendAsync(new()
                {
                    limite = cliente.Limite,
                    saldo = cliente.Saldo
                });
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                ThrowError(ex.Message, 422);
            }

        }
    }
}