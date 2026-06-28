using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Command.RevogarDispositivo;

public sealed class RevogarDispositivoCommandValidation : AbstractValidator<RevogarDispositivoCommand>
{
    public RevogarDispositivoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
