using BLZ.Client.ViewModels;

namespace BLZ.Client.Views;

public partial class CategoryPage : ContentPage
{
	public CategoryPage(CategoryViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}