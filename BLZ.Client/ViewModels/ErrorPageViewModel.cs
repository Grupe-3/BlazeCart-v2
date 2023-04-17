using BLZ.Client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BLZ.Client.ViewModels
{
    public partial class ErrorPageViewModel : ObservableObject
    {
        [RelayCommand]
        async void OnReturnHome(object obj)
        {
            await Shell.Current.GoToAsync(nameof(HomePage));
        }

    }
}
