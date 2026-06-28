namespace Acme.Sistemas.Atena.Mobile.Services.Platform;

public interface ICameraService
{
    Task<bool> TemCameraAsync();
    Task<byte[]?> CapturarFotoAsync();
}

public sealed class CameraService : ICameraService
{
    public Task<bool> TemCameraAsync()
        => Task.FromResult(MediaPicker.Default.IsCaptureSupported);

    public async Task<byte[]?> CapturarFotoAsync()
    {
        if (!MediaPicker.Default.IsCaptureSupported) return null;
        var foto = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
        {
            Title = "Tire uma foto para a batida"
        });
        if (foto is null) return null;

        using var stream = await foto.OpenReadAsync();
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        return ms.ToArray();
    }
}
