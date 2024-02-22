using RinhaBackEnd2024.Persistence.Interfaces;

namespace Endpoints.ClientesExtrato
{
    public class Endpoint(IClienteRepository _clienteRepository, ITransacaoRepository _transacaoRepository) : Endpoint<Request, Response, Mapper>
    {
        public override void Configure()
        {
            Get("/clientes/{id}/extrato");
            AllowAnonymous();
            DontThrowIfValidationFails();
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            if (r.id < 1 || r.id > 5)
                ThrowError("Cliente não encontrado", 404);

            var cliente = await _clienteRepository.BuscarCliente(r.id);

            if (cliente == null)
            {
                await SendNotFoundAsync();
                return;
            }

            var transacoes = await _transacaoRepository.BuscarUltimasTransacoes(r.id);

            await SendAsync(new Response()
            {
                saldo = new Saldo()
                {
                    total = cliente.Saldo,
                    limite = cliente.Limite
                },
                ultimas_transacoes = transacoes
            });
        }
    }
}