using BLZ.Client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BLZ.Client.ViewModels
{
    public partial class WelcomePage2ViewModel : ObservableObject
    {
        [RelayCommand]
        async void Next(object obj)
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
    }
}
