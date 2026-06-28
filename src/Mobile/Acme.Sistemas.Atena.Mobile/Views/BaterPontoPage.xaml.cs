using Acme.Sistemas.Atena.Mobile.ViewModels;

namespace Acme.Sistemas.Atena.Mobile.Views;

public partial class BaterPontoPage : ContentPage
{
    public BaterPontoPage(BaterPontoViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
