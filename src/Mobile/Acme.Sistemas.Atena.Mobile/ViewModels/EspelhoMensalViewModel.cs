using Acme.Sistemas.Atena.Mobile.Services;
using Acme.Sistemas.Atena.Mobile.Services.Api;
using Acme.Sistemas.Atena.Mobile.Shared.Dtos;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Acme.Sistemas.Atena.Mobile.ViewModels;

public partial class EspelhoMensalViewModel : ObservableObject
{
    private readonly IAtenaApi _api;
    private readonly ISecureTokenStore _tokens;

    public EspelhoMensalViewModel(IAtenaApi api, ISecureTokenStore tokens)
    {
        _api = api;
        _tokens = tokens;
    }

    [ObservableProperty] private string _competencia = DateTime.Now.ToString("yyyy-MM");
    [ObservableProperty] private EspelhoMensalDto? _espelho;
    [ObservableProperty] private bool _carregando;
    [ObservableProperty] private string? _erro;
    [ObservableProperty] private EspelhoDiaDto? _diaSelecionado;

    [RelayCommand]
    private async Task CarregarAsync()
    {
        Carregando = true;
        Erro = null;
        try
        {
            var funcId = await _tokens.GetAsync("atena.funcionarioId") ?? string.Empty;
            if (string.IsNullOrEmpty(funcId))
            {
                Erro = "Funcionário não identificado.";
                return;
            }
            var resp = await _api.ObterEspelhoAsync(funcId, Competencia);
            Espelho = resp.Espelho;
        }
        catch (Refit.ApiException ex)
        {
            Erro = $"Erro {(int)ex.StatusCode}: {ex.Message}";
        }
        finally
        {
            Carregando = false;
        }
    }

    [RelayCommand]
    private void SelecionarDia(EspelhoDiaDto? dia) => DiaSelecionado = dia;

    [RelayCommand]
    private async Task SolicitarAjusteParaBatidaAsync(EspelhoBatidaDto? batida)
    {
        if (batida is null || DiaSelecionado is null) return;
        var page = Application.Current?.Windows[0]?.Page;
        if (page is null) return;

        var novaHoraTxt = await page.DisplayPromptAsync(
            "Solicitar ajuste",
            $"Hora correta para {batida.Tipo} ({batida.Hora}):",
            "Solicitar", "Cancelar", initialValue: batida.Hora);
        if (string.IsNullOrWhiteSpace(novaHoraTxt) ||
            !TimeSpan.TryParse(novaHoraTxt, out var hora)) return;

        var motivo = await page.DisplayPromptAsync(
            "Justificativa", "Informe o motivo:", "Enviar", "Cancelar");
        if (string.IsNullOrWhiteSpace(motivo)) return;

        var novaData = DiaSelecionado.Data.ToDateTime(TimeOnly.MinValue).Add(hora);
        try
        {
            await _api.SolicitarAjusteAsync(new SolicitarAjusteRequest(
                batida.Id,
                TipoAjusteDto.AlteracaoHora,
                novaData,
                null,
                motivo,
                null));
            await page.DisplayAlert("Ajuste", "Solicitação enviada.", "OK");
        }
        catch (Refit.ApiException ex)
        {
            await page.DisplayAlert("Ajuste", $"Erro {(int)ex.StatusCode}", "OK");
        }
    }
}
