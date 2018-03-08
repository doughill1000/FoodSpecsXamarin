using Plugin.SecureStorage;
using Xamarin.Forms;
using FoodSpecs.PCL;
using Ninject;
using System;
using System.Linq;
using Plugin.Geolocator;
using System.Threading.Tasks;

namespace FoodSpecs
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			CrossSecureStorage.Current.DeleteKey(LSConstants.AccessToken);
			CrossSecureStorage.Current.DeleteKey(LSConstants.Latitude);
			CrossSecureStorage.Current.DeleteKey(LSConstants.Longitude);
			CrossSecureStorage.Current.DeleteKey(LSConstants.Restaurants);
			CrossSecureStorage.Current.DeleteKey(LSConstants.Specs);
			CrossSecureStorage.Current.DeleteKey(LSConstants.YelpToken);

			CrossSecureStorage.Current.SetValue(LSConstants.RefreshRestaurants, "False");
			CrossSecureStorage.Current.SetValue(LSConstants.RefreshHome, "False");

			CrossSecureStorage.Current.SetValue(LSConstants.UserName, Constants.DefaultUserName);
			CrossSecureStorage.Current.SetValue(LSConstants.Password, Constants.Password);

			Utils.SetJsonSettings();

			MainPage = new LoadingPage();
		}

		protected override async void OnStart()
		{

		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override async void OnResume()
		{
			// Handle when your app resumes
			CrossSecureStorage.Current.DeleteKey(LSConstants.AccessToken);
			CrossSecureStorage.Current.DeleteKey(LSConstants.YelpToken);
		}
	}
}
