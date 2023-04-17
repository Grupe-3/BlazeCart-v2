using BLZ.Client.ViewModels;

namespace BLZ.Client.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}