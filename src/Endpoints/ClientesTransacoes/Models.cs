using FluentValidation;

namespace Endpoints
{
    public class Request
    {
        public int id { get; set; }

        [FromBody]
        public TransacaoRequest transacao { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.id)
                .NotEmpty()
                .WithMessage("O id do cliente é obrigatório");

            RuleFor(x => x.transacao)
                .NotNull()
                .WithMessage("A transação é obrigatória");

            RuleFor(x => x.transacao.valor)
                .NotEmpty()
                .WithMessage("O valor da transação é obrigatória")
                .GreaterThan(0)
                .WithMessage("O valor da transação deve ser maior que 0");

            RuleFor(x => x.transacao.tipo)
                .NotEmpty()
                .WithMessage("O tipo da transação é obrigatório")
                .Must(x => x == 'd' || x == 'c')
                .WithMessage("O tipo da transação deve ser 'd' ou 'c'");

            RuleFor(x => x.transacao.descricao)
                .NotEmpty()
                .WithMessage("A descrição da transação é obrigatória")
                .MaximumLength(10)
                .WithMessage("A descrição da transação deve ter no máximo 10 caracteres");
        }
    }

    public class Response
    {
        public int limite { get; set; }
        public int saldo { get; set; }
    }

    public class TransacaoRequest
    {
        public int valor { get; set; }
        public char tipo { get; set; }
        public string descricao { get; set; }
    }

}
