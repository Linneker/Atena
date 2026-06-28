namespace Acme.Sistemas.Atena.Mobile.Services.Platform;

public interface IDeviceCapabilityHelper
{
    Task<DeviceCapabilities> InspecionarAsync();
}

public sealed record DeviceCapabilities(
    bool TemCamera,
    bool TemBiometria,
    string Plataforma,
    string Modelo,
    string OsVersion);

public sealed class DeviceCapabilityHelper : IDeviceCapabilityHelper
{
    private readonly ICameraService _camera;
    private readonly IBiometriaService _bio;

    public DeviceCapabilityHelper(ICameraService camera, IBiometriaService bio)
    {
        _camera = camera;
        _bio = bio;
    }

    public async Task<DeviceCapabilities> InspecionarAsync()
    {
        var temCam = await _camera.TemCameraAsync();
        var temBio = await _bio.SuportaBiometriaAsync();
        return new DeviceCapabilities(
            TemCamera: temCam,
            TemBiometria: temBio,
            Plataforma: DeviceInfo.Current.Platform.ToString(),
            Modelo: DeviceInfo.Current.Model,
            OsVersion: DeviceInfo.Current.VersionString);
    }
}
