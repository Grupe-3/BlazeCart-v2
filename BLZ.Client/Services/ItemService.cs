using BLZ.Client.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using BLZ.Client.Refit;

namespace BLZ.Client.Services;

public class ItemService
{
    private readonly IItemApi _api;

    ObservableCollection<Item> _itemList = new();
    public ObservableCollection<Item> CartItems { get; set; } = new();

    public event EventHandler<CartUsedEventArgs> CartUsed;

    public event EventHandler<EventArgs> CartTbUpdated;

    public event EventHandler<EventArgs> FavTbUpdated;

    public event EventHandler<CartUsedEventArgs> CheapestCart;

    public double percentDifference;

    public ItemService(IItemApi api)
    {
        _api = api;
    }

    public async Task<ObservableCollection<Item>> Get(int index, int count)
    {
        var items = await _api.GetRange(index, count);
        return new ObservableCollection<Item>(items);
    }
    public async Task<ObservableCollection<Item>> GetCheapestItems(ObservableCollection<Item> items, bool mixed = false)
    {
        ObservableCollection<Item> cheapestItemsIKI = new ObservableCollection<Item>();
        ObservableCollection<Item> cheapestItemsBarbora = new ObservableCollection<Item>();
        ObservableCollection<Item> cheapestItemsMixed = new ObservableCollection<Item>();

        double totalPriceIKI = 0;
        double totalPriceBarbora = 0;
        foreach (var item in items)
        {
            string name = item.NameLT;
            string? category = item.RemappedCategoryName;
            double price = item.Price;
            int comparedMerch = (int)item.Merchant;
            double amount = 0;
            if (item.Amount != null) { amount = (double)item.Amount; }
            Uri image = item.Image;

            if (mixed)
            {
                var itemMixed = await _api.GetCheapest(name, category, price, amount, 2, comparedMerch);

                if (itemMixed.NameLT == name)
                    itemMixed.Image = image;

                cheapestItemsMixed.Add(itemMixed);
            }
            else
            {
                var itemIKI = await _api.GetCheapest(name, category, price, amount, 0, comparedMerch);
                
                if (itemIKI.Image == null)
                    itemIKI.Image = image;
                if (itemIKI.Merchant != Common.Models.Merchant.BARBORA) {
                    cheapestItemsIKI.Add(itemIKI);

                    var itemBarbora = await _api.GetCheapest(name, category, price, amount, 1, comparedMerch);
                    if (itemBarbora.Image == null)
                        itemBarbora.Image = image;
                    if (itemBarbora.Merchant != Common.Models.Merchant.BARBORA)
                        cheapestItemsBarbora.Add(itemBarbora);
                }
                else
                {
                    cheapestItemsBarbora.Add(itemIKI);
                }
                  
            }
          
        }
        if (cheapestItemsBarbora.Count > 0)
        {
            foreach (var i in cheapestItemsBarbora)
            {
                totalPriceBarbora += i.Price;
            }
        }

        if (cheapestItemsIKI.Count > 0)
        {
            foreach (var i in cheapestItemsIKI)
            {
                totalPriceIKI += i.Price;
            }
        }
        if (mixed)
            return cheapestItemsMixed;

        if (totalPriceIKI < totalPriceBarbora && cheapestItemsIKI.Count == items.Count)
        {
            percentDifference = (1 - totalPriceIKI / totalPriceBarbora) / 100;
            return cheapestItemsIKI;

        }
        else if (totalPriceBarbora < totalPriceIKI && cheapestItemsBarbora.Count == items.Count)
        {
            percentDifference = (1 - totalPriceBarbora / totalPriceIKI) / 100;
            return cheapestItemsBarbora;
        }
        else if (totalPriceIKI >= totalPriceBarbora && cheapestItemsBarbora.Count != items.Count && cheapestItemsIKI.Count == items.Count)
            return cheapestItemsIKI;
        else if (totalPriceBarbora >= totalPriceIKI && cheapestItemsIKI.Count != items.Count && cheapestItemsBarbora.Count == items.Count)
            return cheapestItemsBarbora;
        else if (cheapestItemsIKI.Count != items.Count && cheapestItemsBarbora.Count != items.Count)
        {
            return null;
        }
        else
            return null;
    }
    public async Task<ObservableCollection<Item>> GetItems(string fileName)
    {

        if (_itemList.Count > 0) {
            return _itemList;
        }

        await using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
        using StreamReader r = new(stream);
        string json = await r.ReadToEndAsync();
        var jobj = JObject.Parse(json);
        _itemList = JsonConvert.DeserializeObject<ObservableCollection<Item>>(jobj["shopItems"].ToString());
        return _itemList;
    }
  
    public void PutItems(ICollection<Item> items)
    {
        CartItems.Clear();
        foreach (var value in items)
        {
            CartItems.Add(value);
        }
        OnCartChanged(new CartUsedEventArgs(CartItems));

    }
    public void AddToCart(Item item)
    {
        var query = CartItems.Where(x => x.NameLT == item.NameLT && x.Merchant == item.Merchant);
        var result = query.ToList();
        if (result.Count == 0)
        {
            CartItems.Add(item);
        }
        else
        {
        }
        OnCartChanged(new CartUsedEventArgs(CartItems));
    }
    public void RemoveFromCart(Item item)
    {
        CartItems.Remove(item);
    }
    public virtual void OnCartChanged(CartUsedEventArgs e)
    {
        if (CartUsed != null) CartUsed(this, e);
    }

    public virtual void OnCartTbUpdated(EventArgs e)
    {
        if (CartTbUpdated != null) CartTbUpdated(this, e);
    }
    public virtual void OnFavTbUpdated(EventArgs e)
    {
        if (FavTbUpdated != null) FavTbUpdated(this, e);
    }
    public virtual void OnCheapestCart(CartUsedEventArgs e)
    {
        if (CheapestCart != null) CheapestCart(this, e);
    }
}
