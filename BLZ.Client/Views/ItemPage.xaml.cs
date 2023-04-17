using BLZ.Client.ViewModels;

namespace BLZ.Client.Views;

public partial class ItemPage : ContentPage
{
	public ItemPage(ItemPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

}