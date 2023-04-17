using BLZ.Client.ViewModels;
namespace BLZ.Client.Views;

public partial class EmptyStorePage : ContentPage
{
	public EmptyStorePage(EmptyStorePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}