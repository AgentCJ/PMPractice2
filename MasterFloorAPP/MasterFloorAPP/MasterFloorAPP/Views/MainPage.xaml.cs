using MasterFloorAPP.ViewModels;

namespace MasterFloorAPP;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainPageViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as MainPageViewModel)?.LoadPartnersCommand?.Execute(null);
    }
}