namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Command.RegistrarDispositivo;

public sealed record RegistrarDispositivoCommandResult(Guid DispositivoId, bool JaExistia);
