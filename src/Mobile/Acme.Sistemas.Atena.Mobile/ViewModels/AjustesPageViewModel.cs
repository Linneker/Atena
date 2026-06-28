using Acme.Sistemas.Atena.Mobile.Services.Api;
using Acme.Sistemas.Atena.Mobile.Shared.Dtos;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Acme.Sistemas.Atena.Mobile.ViewModels;

public partial class AjustesPageViewModel : ObservableObject
{
    private readonly IAtenaApi _api;

    public AjustesPageViewModel(IAtenaApi api)
    {
        _api = api;
    }

    [ObservableProperty] private List<AjusteDto> _ajustes = new();
    [ObservableProperty] private bool _carregando;
    [ObservableProperty] private string? _erro;

    [RelayCommand]
    private async Task CarregarAsync()
    {
        Carregando = true;
        Erro = null;
        try
        {
            var resp = await _api.ListarMeusAjustesAsync();
            Ajustes = resp.Items.ToList();
        }
        catch (Refit.ApiException ex) { Erro = ex.Message; }
        finally { Carregando = false; }
    }

    [RelayCommand]
    private async Task SolicitarNovoAjusteAsync()
    {
        var motivo = await Shell.Current.DisplayPromptAsync(
            "Solicitar ajuste", "Descreva o motivo do ajuste:", "Solicitar", "Cancelar",
            placeholder: "Ex.: Esqueci de bater a saída ontem", maxLength: 2000);
        if (string.IsNullOrWhiteSpace(motivo)) return;

        try
        {
            await _api.SolicitarAjusteAsync(new SolicitarAjusteRequest(
                MarcacaoOriginalId: null,
                TipoAjuste: TipoAjusteDto.Justificativa,
                DataHoraProposta: null,
                TipoMarcacaoProposta: null,
                Motivo: motivo,
                AnexoUrl: null));
            await CarregarAsync();
        }
        catch (Refit.ApiException ex) { Erro = ex.Message; }
    }
}
