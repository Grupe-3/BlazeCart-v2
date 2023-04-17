using BLZ.Client.ViewModels;

namespace BLZ.Client.Views;

public partial class GoogleMaps : ContentPage
{
	public GoogleMaps(GoogleMapsViewModel vm)
	{
		InitializeComponent();
		BindingContext= vm;
	}
}