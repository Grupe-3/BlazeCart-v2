﻿using BlazeCart.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeCart.Services;
using System.Diagnostics;
using System.Windows.Input;
using BlazeCart.Views;
using Debug = System.Diagnostics.Debug;

//Inserting service
namespace BlazeCart.ViewModels;


public class ItemsViewModel : BaseViewModel
{
    //A collection that notifies when it is changed.
    public ObservableCollection<Item> Items { get; set; } = new();

    public ObservableCollection<Item> CartItems { get; set; } = new();

    public Item SelectedItem { get; set; }

    private Cart cart = new Cart();
    
    public Command<Item> CartCommand { get; set; }
    ItemService _itemService = new();
    CartService _cartService = new();

    private INavigation _navigation;
    //Dependency injection
    public ItemsViewModel(INavigation navigation) {
        //Paremtrize the parameters?
        name = "Item finder";
        Items = new ObservableCollection<Item>();
        GetItemsAsync();
        CartCommand = new Command<Item>(OnCartCommand);
    }



    async void GetItemsAsync() {

        if (IsBusy)
        {
            return;
        }
        try
        {
            isBusy = true;
            //Loading items from service
            var items = await _itemService.GetItems();

            //Clears the local collection
            if (Items.Count != 0) {
                Items.Clear();
            }

            //Adds items to the local collection
            foreach( var item in items)
                Items.Add(item);
        }

        catch (Exception ex)
        {
            //Logs error
            Debug.WriteLine($"Unable to get items: {  ex.Message}");
            await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "OK");
        }

        finally {
            isBusy = false;
        }

    }

     async void OnCartCommand(Item item)
    {
        this.SelectedItem = item;
       this.CartItems.Add(SelectedItem);
       //await _cartService.ItemsToCart( this.CartItems, "Vau pavyko"); //error on this method
        //await Application.Current.MainPage.DisplayAlert("lol", "ok", "nice");

    }
  
}
