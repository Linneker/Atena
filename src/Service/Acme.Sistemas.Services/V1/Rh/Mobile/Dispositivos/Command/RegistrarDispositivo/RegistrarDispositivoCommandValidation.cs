using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Command.RegistrarDispositivo;

public sealed class RegistrarDispositivoCommandValidation : AbstractValidator<RegistrarDispositivoCommand>
{
    public RegistrarDispositivoCommandValidation()
    {
        RuleFor(x => x.DeviceId).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Modelo).NotEmpty().MaximumLength(120);
        RuleFor(x => x.OsVersion).MaximumLength(40);
        RuleFor(x => x.AppVersion).MaximumLength(20);
        RuleFor(x => x.PushToken).MaximumLength(500);
    }
}
