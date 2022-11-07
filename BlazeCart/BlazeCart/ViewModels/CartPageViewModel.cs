﻿using System.Collections.ObjectModel;
using BlazeCart.Models;
using BlazeCart.Services;
using BlazeCart.Views;
using CommunityToolkit.Mvvm.Input;

namespace BlazeCart.ViewModels
{
    public partial class CartPageViewModel : BaseViewModel
    {
        private readonly DataService _cartService;

        public Cart Cart { get; set; }

        public int ItemCount { get; set; }

        public CartHistoryPageViewModel _vm;
        public ObservableCollection<Item> CartItems { get; set; } = new();

        public CartPageViewModel(DataService cartService, CartHistoryPageViewModel vm)
        {
            _vm = vm;
            _vm.CartUsed += CartUsedEventHandler;
            _cartService = cartService;
        }

        private void CartUsedEventHandler(object sender, EventArgs e)
        {
            CartItems.Clear();
            foreach (var item in _vm.CartItems)
            {
                CartItems.Add(item);
            }
        }

        [RelayCommand]
        void Remove(Item item)
        {
            CartItems.Remove(item);
        }

        [RelayCommand]
        async void Save(object obj)
        {
            if(CartItems.Count > 0)
            {
                string cartName = await Shell.Current.DisplayPromptAsync("Išsaugoti krepšelį", "Įveskite krepšelio pavadinimą: ", "OK",
               "Cancel", "Įveskite pavadinimą...");
                
                await _cartService.AddCartToDb(cartName, CartItems, CartItems.Count, GetCartPrice(CartItems));
                await _vm.Refresh();
            }
            else
            {
                await Shell.Current.DisplayAlert("Klaida!", "Krepšelis tuščias!", "OK");
            }
        }


        [RelayCommand]
        async void CheapestStore(object obj)
        {
            await Shell.Current.GoToAsync(nameof(CheapestStorePage));
        }

        private Double GetCartPrice(ObservableCollection<Item> cartItems)
        {
            Double TotalPrice = 0;
            foreach (Item I in cartItems)
            {
                TotalPrice += I.Price;
            }
            return TotalPrice;
        }
    }
}
