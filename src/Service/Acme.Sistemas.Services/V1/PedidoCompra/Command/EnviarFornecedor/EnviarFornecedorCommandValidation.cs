using FluentValidation;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Command.EnviarFornecedor;

public sealed class EnviarFornecedorCommandValidation : AbstractValidator<EnviarFornecedorCommand>
{
    public EnviarFornecedorCommandValidation()
    {
        RuleFor(x => x.PedidoId).NotEmpty();
        RuleFor(x => x.EmailDestinoOverride).EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.EmailDestinoOverride));
    }
}
