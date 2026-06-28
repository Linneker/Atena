using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Command.RemoverJornada;

public sealed class RemoverJornadaCommandValidation : AbstractValidator<RemoverJornadaCommand>
{
    public RemoverJornadaCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
