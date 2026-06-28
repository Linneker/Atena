using Acme.Sistemas.Atena.Mobile.ViewModels;

namespace Acme.Sistemas.Atena.Mobile.Views;

public partial class EspelhoMensalPage : ContentPage
{
    public EspelhoMensalPage(EspelhoMensalViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
