using Acme.Sistemas.Atena.Mobile.ViewModels;

namespace Acme.Sistemas.Atena.Mobile.Views;

public partial class PrimeiroAcessoPage : ContentPage
{
    public PrimeiroAcessoPage(PrimeiroAcessoViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
