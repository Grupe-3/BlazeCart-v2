using BLZ.Client.ViewModels;
using Newtonsoft.Json;

namespace BLZ.Client.Views;

public partial class HomePage : ContentPage
{
	public HomePage(HomePageViewModel vm)
	{
        BindingContext = vm;
        GetProfileInfo();
        InitializeComponent();
        
    }

    private void GetProfileInfo()
    {
        var userInfo = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("FreshFirebaseToken", ""));
        //DisplayName.Text = userInfo.User.DisplayName;

    }
}