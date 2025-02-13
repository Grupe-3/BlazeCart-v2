﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BLZ.Client.ViewModels;

public partial class EmptyStorePageViewModel : ObservableObject
{
    [RelayCommand]
    async void BackToCart(object obj)
    {
        await Shell.Current.GoToAsync("..");
    }
}
