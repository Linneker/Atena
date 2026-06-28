using Acme.Sistemas.Atena.Mobile.ViewModels;

namespace Acme.Sistemas.Atena.Mobile.Views;

public partial class AjustesPage : ContentPage
{
    public AjustesPage(AjustesPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AjustesPageViewModel vm) await vm.CarregarCommand.ExecuteAsync(null);
    }
}
