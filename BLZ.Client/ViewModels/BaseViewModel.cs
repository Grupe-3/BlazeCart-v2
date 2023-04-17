using CommunityToolkit.Mvvm.ComponentModel;
namespace BLZ.Client.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    public string name;

    [ObservableProperty]
    public bool isBusy;

}
