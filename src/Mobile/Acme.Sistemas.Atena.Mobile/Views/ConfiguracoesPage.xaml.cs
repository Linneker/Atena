using Acme.Sistemas.Atena.Mobile.ViewModels;

namespace Acme.Sistemas.Atena.Mobile.Views;

public partial class ConfiguracoesPage : ContentPage
{
    public ConfiguracoesPage(ConfiguracoesViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
