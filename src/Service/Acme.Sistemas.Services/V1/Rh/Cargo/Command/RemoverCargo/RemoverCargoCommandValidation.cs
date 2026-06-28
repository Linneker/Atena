using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Command.RemoverCargo;

public sealed class RemoverCargoCommandValidation : AbstractValidator<RemoverCargoCommand>
{
    public RemoverCargoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
