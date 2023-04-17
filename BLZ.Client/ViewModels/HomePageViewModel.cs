using BLZ.Client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BLZ.Client.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {

        [RelayCommand]
        async void SearchItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(CategoryPage));
        }
        [RelayCommand]
        async void CartHistory(object obj)
        {
            await Shell.Current.GoToAsync(nameof(CartHistoryPage));
        }
        [RelayCommand]
        async void FavoriteItems(object obj)
        {
            await Shell.Current.GoToAsync(nameof(FavoriteItemPage));
        }

        
    }
}

