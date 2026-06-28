using Acme.Sistemas.Atena.Mobile.ViewModels;

namespace Acme.Sistemas.Atena.Mobile.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
