using System.Collections.ObjectModel;
using BLZ.Client.Models;
using BLZ.Client.Services;
using BLZ.Client.ViewModels;

namespace BLZ.Client.Views;

public partial class FavoriteItemPage : ContentPage
{
	public FavoriteItemPage(FavoriteItemViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
    }
}