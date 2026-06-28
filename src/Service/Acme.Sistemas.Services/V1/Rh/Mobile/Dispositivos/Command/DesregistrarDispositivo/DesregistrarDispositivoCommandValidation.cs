using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Command.DesregistrarDispositivo;

public sealed class DesregistrarDispositivoCommandValidation : AbstractValidator<DesregistrarDispositivoCommand>
{
    public DesregistrarDispositivoCommandValidation()
    {
        RuleFor(x => x.DeviceId).NotEmpty().MaximumLength(120);
    }
}
