namespace Acme.Sistemas.Atena.Mobile.Services;

public interface INotificationService
{
    Task MostrarErroAsync(string mensagem);
    Task MostrarSucessoAsync(string mensagem);
    Task<bool> ConfirmarAsync(string titulo, string mensagem, string ok = "OK", string cancelar = "Cancelar");
}

public sealed class NotificationService : INotificationService
{
    public Task MostrarErroAsync(string mensagem)
        => Application.Current?.Windows[0]?.Page?.DisplayAlert("Erro", mensagem, "OK") ?? Task.CompletedTask;

    public Task MostrarSucessoAsync(string mensagem)
        => Application.Current?.Windows[0]?.Page?.DisplayAlert("OK", mensagem, "Fechar") ?? Task.CompletedTask;

    public Task<bool> ConfirmarAsync(string titulo, string mensagem, string ok = "OK", string cancelar = "Cancelar")
        => Application.Current?.Windows[0]?.Page?.DisplayAlert(titulo, mensagem, ok, cancelar) ?? Task.FromResult(false);
}
