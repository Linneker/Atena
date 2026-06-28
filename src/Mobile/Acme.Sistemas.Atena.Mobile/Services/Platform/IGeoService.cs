namespace Acme.Sistemas.Atena.Mobile.Services.Platform;

public interface IGeoService
{
    Task<(decimal? Latitude, decimal? Longitude)?> ObterCoordenadaAtualAsync();
}

public sealed class GeoService : IGeoService
{
    public async Task<(decimal? Latitude, decimal? Longitude)?> ObterCoordenadaAtualAsync()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.Default.GetLocationAsync(request);
            if (location is null) return null;
            return ((decimal)location.Latitude, (decimal)location.Longitude);
        }
        catch (FeatureNotEnabledException) { return null; }
        catch (FeatureNotSupportedException) { return null; }
        catch (PermissionException) { return null; }
        catch (Exception) { return null; }
    }
}
