using BLZ.Client.ViewModels;

namespace BLZ.Client.Views;

public partial class CartPage : ContentPage
{
	public CartPage(CartPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}