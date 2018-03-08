using System.Collections.Generic;
using Xamarin.Forms;
using Ninject;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FoodSpecs.PCL;
using Plugin.SecureStorage;
using Acr.UserDialogs;

namespace FoodSpecs
{
	public partial class Restaurants : ContentPage
	{
		IEnumerable<Restaurant> _restaurants;

		public Restaurants()
		{
			InitializeComponent();
			Title = "Restaurants";
			Icon = "restaurants.png";
			NavigationPage.SetBackButtonTitle(this, "");

			lvRestaurants.RefreshCommand = new Command(async () => await RefreshRestaurantList());

			Device.BeginInvokeOnMainThread(async () =>
			{
				await PopulateRestaurants();
			});

			lvRestaurants.ItemTapped += async (sender, e) =>
			{
				((ListView)sender).SelectedItem = null;
				var restaurant = (Restaurant)e.Item;

				NavigationPage.SetBackButtonTitle(this, "");
				await Navigation.PushAsync(new ViewRestaurantSpecs(restaurant));
				//await Navigation.PushAsync(new TabRestaurant(restaurant));
			};
		}

		#region Private Methods

		protected async override void OnAppearing()
		{
			await Task.Run(async () =>
			{
				base.OnAppearing();
				//If specs page is refreshed, this list should get quick refresh
				//await Utils.GetValueLocalStorageAsync(LSConstants.IsRefresh)
				if (await Utils.GetValueLocalStorageAsync(LSConstants.RefreshRestaurants) == "True")
				{
					UserDialogs.Instance.ShowLoading("Refreshing", MaskType.Black);
					Device.BeginInvokeOnMainThread(async () => { await PopulateRestaurants(); });
					CrossSecureStorage.Current.SetValue(LSConstants.RefreshRestaurants, "False");
					UserDialogs.Instance.HideLoading();
				}
			});
		}

		/// <summary>
		/// Refreshes restaurant list using new lat and long
		/// </summary>
		/// <returns>The restaurant list.</returns>
		async Task RefreshRestaurantList(){
			//lvRestaurants.IsRefreshing = true;
			CrossSecureStorage.Current.DeleteKey(LSConstants.Latitude);
			CrossSecureStorage.Current.DeleteKey(LSConstants.Longitude);
			CrossSecureStorage.Current.DeleteKey(LSConstants.Restaurants);
			CrossSecureStorage.Current.DeleteKey(LSConstants.Specs);
			await PopulateRestaurants();
			CrossSecureStorage.Current.SetValue(LSConstants.RefreshHome, "True");
			//lvRestaurants.EndRefresh();
		}

		/// <summary>
		/// Gets restaurants by coordinates.
		/// </summary>
		/// <returns>The restaurants.</returns>
		async Task PopulateRestaurants(){
			lvRestaurants.IsRefreshing = true;
			IRestaurantService _restaurantService = Startup.Container.Get<IRestaurantService>();
			_restaurants = await _restaurantService.SearchRestaurantsByCoordinates();
			lvRestaurants.ItemsSource = new ObservableCollection<Restaurant>(_restaurants);
			lvRestaurants.EndRefresh();
		}

		/// <summary>
		/// Search autocomplete for restaurant by coordinates and text string.
		/// </summary>
		/// <returns>The restaurants.</returns>
		/// <param name="text">Text.</param>
		async void SearchRestaurants(object sender, EventArgs e){

		    var config = new PromptConfig
		    {
		        Title = "Search",
		        OkText = "Search",
		    };

		    var searchTerm = await UserDialogs.Instance.PromptAsync(config);
		    IRestaurantService _restaurantService = Startup.Container.Get<IRestaurantService>();
			//return await _restaurantService.SearchAutoComplete(text).ConfigureAwait(false);

		    var restaurants = await _restaurantService.SearchRestaurantsByCoordinates(searchTerm.Text);

            if (restaurants != null)
            {
                lvRestaurants.ItemsSource = new ObservableCollection<Restaurant>(restaurants);
            }
		}

        /// <summary>
        /// Filtering for restaurants list.
        /// </summary>
        void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e){
			if(string.IsNullOrWhiteSpace(e.NewTextValue)){
			   lvRestaurants.ItemsSource = new ObservableCollection<Restaurant>(_restaurants);
			}else{
				var newList = _restaurants.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()));
				lvRestaurants.ItemsSource = new ObservableCollection<Restaurant>(newList);
				//lvRestaurants.ItemsSource = new ObservableCollection<Restaurant>(await SearchRestaurants(txtSearch.Text));
			}
        }
		#endregion
	}
}
