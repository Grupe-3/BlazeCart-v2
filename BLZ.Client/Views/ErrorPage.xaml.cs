using BLZ.Client.ViewModels;

namespace BLZ.Client.Views;

public partial class ErrorPage : ContentPage
{
	public ErrorPage(ErrorPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}