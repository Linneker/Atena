using FluentValidation;

namespace Acme.Sistemas.Services.V1.Inventario.Command.AbrirInventario;

public sealed class AbrirInventarioCommandValidation : AbstractValidator<AbrirInventarioCommand>
{
    public AbrirInventarioCommandValidation()
    {
        RuleFor(x => x.EstoqueId).NotEmpty();
        RuleFor(x => x.Observacao).MaximumLength(2000);
    }
}
