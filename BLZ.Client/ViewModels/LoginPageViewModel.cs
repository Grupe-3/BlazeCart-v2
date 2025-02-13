﻿using BLZ.Client.Services;
using BLZ.Client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
namespace BLZ.Client.ViewModels
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        public string email;
        [ObservableProperty]
        public string password;
        private readonly AuthService _authService;
        public LoginPageViewModel(AuthService authService)
        {
            _authService = authService;
        }
        [RelayCommand]
        async Task Login(object obj)
        {
            if (IsBusy)
                return;
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await Shell.Current.DisplayAlert("Error!", "Please fill all fields.", "OK");
                return;
            }

            try
            {
                IsBusy = true;
                await _authService.LoginAsync(Email, Password);

                await Shell.Current.GoToAsync(nameof(HomePage));
            }
            catch
            {
                await Shell.Current.DisplayAlert("Klaida!","Neteisingi duomenys!", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async void Register(object obj)
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }
    }
}
