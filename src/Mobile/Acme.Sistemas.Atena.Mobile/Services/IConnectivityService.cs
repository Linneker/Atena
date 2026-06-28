namespace Acme.Sistemas.Atena.Mobile.Services;

public interface IConnectivityService
{
    bool EstaOnline { get; }
    event EventHandler<bool>? StatusMudou;
}

public sealed class ConnectivityService : IConnectivityService, IDisposable
{
    public bool EstaOnline => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

    public event EventHandler<bool>? StatusMudou;

    public ConnectivityService()
    {
        Connectivity.ConnectivityChanged += OnChanged;
    }

    private void OnChanged(object? sender, ConnectivityChangedEventArgs e)
        => StatusMudou?.Invoke(this, e.NetworkAccess == NetworkAccess.Internet);

    public void Dispose() => Connectivity.ConnectivityChanged -= OnChanged;
}
