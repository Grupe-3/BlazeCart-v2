﻿using CommunityToolkit.Mvvm.ComponentModel;
using BLZ.Client.Models;
using BLZ.Client.Services;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace BLZ.Client.ViewModels
{
    [QueryProperty(nameof(Item), "Item")]
    [QueryProperty(nameof(Name), "NameLT")]
    [QueryProperty(nameof(Price), "Price")]
    [QueryProperty(nameof(Image), "Image")]
    [QueryProperty(nameof(Description), "Description")]
    [QueryProperty(nameof(Cat), "Cat")]
    [QueryProperty(nameof(MerchName), "MerchName")]

    public partial class ItemPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        public Item item;
        [ObservableProperty]
        public new string name;

        [ObservableProperty]
        public double price;

        [ObservableProperty]
        public Uri image;

        [ObservableProperty]
        public string cat;

        [ObservableProperty]
        public string description;

        [ObservableProperty]
        public string merchName;

        private ItemService _itemService;

        private DataService _dataService;

        private ILogger<ItemPageViewModel> _logger; 

        public ItemPageViewModel(ItemService itemService, DataService dataService, ILogger<ItemPageViewModel> logger)
        {
            _itemService = itemService;
            _dataService = dataService;
            _logger = logger;
        }

        [RelayCommand]
        async void Cart(object obj)
        {
            _itemService.AddToCart(Item);
            await Shell.Current.DisplayAlert("Įdėta į krepšelį!", "Prekė sėkmingai įdėta į krepšelį!", "OK");
        }

        [RelayCommand]
        async void AddItemToFavorites(object obj)
        {
            try
            {
                await Shell.Current.DisplayAlert("Prekės pridėjimas sėkmingas", "Sėkmingai pažymėjote prekę kaip mėgstamiausią", "OK");
                await _dataService.AddFavoriteItemToDb(Item);
                _itemService.OnFavTbUpdated(EventArgs.Empty);
                _logger.LogInformation($"Item {Item.NameLT} added to favorites");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on adding favorite item to DB");
                await Shell.Current.DisplayAlert("Klaida!", ex.Message, "OK");
                throw;
            }
        }
       
    }
}
