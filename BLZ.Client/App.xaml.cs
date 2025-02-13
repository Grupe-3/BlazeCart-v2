﻿using BLZ.Client.Views;
using MetroLog.Maui;
using MonkeyCache.FileStore;

namespace BLZ.Client;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
        Barrel.ApplicationId = AppInfo.PackageName;
        LogController.InitializeNavigation(
            page => MainPage!.Navigation.PushModalAsync(page),
            () => MainPage!.Navigation.PopModalAsync());
    }
}
