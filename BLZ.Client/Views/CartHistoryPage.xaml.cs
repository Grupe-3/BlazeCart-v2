using BLZ.Client.ViewModels;

namespace BLZ.Client.Views;

public partial class CartHistoryPage : ContentPage
{
	public CartHistoryPage(CartHistoryPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}