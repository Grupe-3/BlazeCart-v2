using BLZ.Client.ViewModels;

namespace BLZ.Client.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}