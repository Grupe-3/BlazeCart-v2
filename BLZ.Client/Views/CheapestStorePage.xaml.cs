using BLZ.Client.ViewModels;

namespace BLZ.Client.Views;

public partial class CheapestStorePage : ContentPage
{
	public CheapestStorePage(CheapestStorePageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}