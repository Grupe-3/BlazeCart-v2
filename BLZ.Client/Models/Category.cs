using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace BLZ.Client.Models;

public class Category : ObservableObject
{
    [JsonProperty]
    public string Name { get; set; }

    [JsonIgnore]
    public int Count { get; set; }

    public Category(string name)
    {
        Name = name;
    }
}
