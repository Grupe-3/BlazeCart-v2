using BLZ.Client.ViewModels;
using MetroLog.Maui;

namespace BLZ.Client.Views;

public partial class ItemCatalogPage : ContentPage
{
    private LogController _logController = new();
    public ItemCatalogPage(ItemsViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    private void Switch_OnToggled(object sender, ToggledEventArgs e)
    {
        _logController.GoToLogsPageCommand.Execute(null);
    }
}
