using System.Collections.ObjectModel;
using BLZ.Client.Models;

namespace BLZ.Client.Services
{
    public class CartUsedEventArgs : EventArgs
    {
        public ObservableCollection<Item> Items { get; }
        public CartUsedEventArgs(ObservableCollection<Item> items)
        {
            Items = items;
        }
    }
}
