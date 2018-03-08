using System;
using System.Collections.Generic;
using FoodSpecs.PCL;
using Ninject;
using Xamarin.Forms;

namespace FoodSpecs
{
	public partial class LoadingPage : ContentPage
	{
		public LoadingPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.White;
			Device.BeginInvokeOnMainThread(async () =>
			{
				//try
				//{
					IsBusy = true;
					var coordinates = await Utils.GetLatAndLong();
					LoadingMessage.Text = "Loading Restaurants...";
					IFoodSpecialService _foodSpecService = Startup.Container.Get<IFoodSpecialService>();
					var foodSpecs = await _foodSpecService.GetFoodSpecsByCoordinates();
					IsBusy = false;
					LoadingContent.IsVisible = false;
					await Navigation.PushModalAsync(new FoodSpecsPage(), false);
				//}catch(Exception ex){
				//	await DisplayAlert("Error", ex.Message, "cancel");
				//	//await DisplayAlert("Error", "There was an error with the web service, please try again later", "Cancel");
				//}

			});
		}
	}
}
