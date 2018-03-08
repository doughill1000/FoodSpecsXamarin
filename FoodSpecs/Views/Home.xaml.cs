using System.Collections.ObjectModel;
using Xamarin.Forms;
using FoodSpecs.PCL;
using Ninject;
using System.Linq;
using System;
using System.Collections.Generic;
using Plugin.SecureStorage;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace FoodSpecs
{
	public partial class Home : ContentPage
	{
		#region Properties
		IEnumerable<FoodSpecial> _allSpecs;
		IEnumerable<FoodSpecial> _listSpecs;
		IEnumerable<FoodSpecial> _sortedSpecs;
		SpecSort _specSort = SpecSort.Upcoming;
		DayOfWeek _daySort = DayOfWeek.Sunday;
		int offset = 25;
		#endregion

		#region Constants
		const string cHomeTitle = "Nearby Specs";
		#endregion

		#region Constructor
		public Home()
		{
			//Icon = "utensils.png";
			Title = cHomeTitle + " - Coming Up";
			InitializeComponent();

			lvUpcomingSpecs.RefreshCommand = new Command(async () => await RefreshSpecsList());
			NavigationPage.SetBackButtonTitle(this, "");

			Device.BeginInvokeOnMainThread(async () =>
			{
				await PopulateSpecsList();
				await SetUpcoming(); //Default
			});

			#region Item Events

			lvUpcomingSpecs.ItemTapped += async (sender, e) =>
			{
				((ListView)sender).SelectedItem = null;
				var spec = (FoodSpecial)e.Item;

				await Navigation.PushAsync(new ManageFoodSpecial(spec));
							//await Navigation.PushAsync(new TabRestaurant(restaurant));
			};

			lvUpcomingSpecs.ItemAppearing +=  (sender, e) =>
			{
				var items = ((ListView)sender).ItemsSource as IList<FoodSpecial>;
				var spec = (FoodSpecial)e.Item;
				var item = items[Math.Max(items.Count - 4, 0)];
				if (items != null && spec.SpecialId == item.SpecialId)
				{
					//Load more items here 
					_listSpecs.ToList().AddRange(_sortedSpecs.Skip(offset).Take(offset));
					offset += offset;
				}
			};

			#endregion

		}
		#endregion

		#region Overrides
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			await Task.Run(async () =>
			{
				base.OnAppearing();
				if (await Utils.GetValueLocalStorageAsync(LSConstants.RefreshHome) == "True")
				{
					UserDialogs.Instance.ShowLoading("Refreshing", MaskType.Black);
					Device.BeginInvokeOnMainThread(async () => { await PopulateSpecsList(); });
					CrossSecureStorage.Current.SetValue(LSConstants.RefreshHome, "False");
					UserDialogs.Instance.HideLoading();
				}
			});
		}
		#endregion

		#region List Methods
		/// <summary>
		/// Refreshes restaurant list using new lat and long
		/// </summary>
		/// <returns>The restaurant list.</returns>
		async Task RefreshSpecsList()
		{
			lvUpcomingSpecs.IsRefreshing = true;
			#region Delete Storage
			CrossSecureStorage.Current.DeleteKey(LSConstants.Latitude);
			CrossSecureStorage.Current.DeleteKey(LSConstants.Longitude);
			CrossSecureStorage.Current.DeleteKey(LSConstants.Restaurants);
			CrossSecureStorage.Current.DeleteKey(LSConstants.Specs);
			#endregion
			await PopulateSpecsList();
			lvUpcomingSpecs.EndRefresh();
			CrossSecureStorage.Current.SetValue(LSConstants.RefreshRestaurants, "True");
			offset = 25;
		}

		async Task PopulateSpecsList()
		{
			IFoodSpecialService _foodSpecService = Startup.Container.Get<IFoodSpecialService>();
			_allSpecs = await _foodSpecService.GetFoodSpecsByCoordinates();
			switch (_specSort)
			{
				case SpecSort.Upcoming:
					await SetUpcoming();
					break;
				case SpecSort.ByDay:
					await SetByDay(_daySort);
					break;
			}
		}
		#endregion

		#region Filter 
		/// <summary>
		/// Callback for text change in filter
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void Filter_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(e.NewTextValue))
			{
				lvUpcomingSpecs.ItemsSource = new ObservableCollection<FoodSpecial>(_listSpecs);
			}
			else {
				//Search on title and description. Maybe update later...
				var newList = _sortedSpecs.Where(x => x.Title.ToLower().Contains(e.NewTextValue.ToLower()) || 
				                               (x.Description != null && x.Description.ToLower().Contains(e.NewTextValue.ToLower())));
				lvUpcomingSpecs.ItemsSource = new ObservableCollection<FoodSpecial>(newList.Take(offset));
			}
		}
		#endregion

		#region Sort
		/// <summary>
		/// Sorts the specs using a display action sheet.
		/// </summary>
		async void SortSpecs(object sender, EventArgs e)
		{
			var action = await DisplayActionSheet("Sort Nearby Specs", "Cancel", null, new string[] { "Coming up", "By Day" });
			switch (action)
			{
				case "Coming up":
					UserDialogs.Instance.ShowLoading("Sorting", MaskType.Black);
					Title = cHomeTitle + " - Coming Up";
					var isUpcomingSorted = await SetUpcoming();
					if (isUpcomingSorted) UserDialogs.Instance.HideLoading();
					break;

				case "By Day":
					var day = await DisplayActionSheet("Days", "Cancel", null,
							   new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" });
					UserDialogs.Instance.ShowLoading("Sorting", MaskType.Black);

					#region Day Switch
					switch (day)
					{
						case "Sunday":
							Title = cHomeTitle + " - Sunday";
							_daySort = DayOfWeek.Sunday;
							break;
						case "Monday":
							Title = cHomeTitle + " - Monday";
							_daySort = DayOfWeek.Monday;
							break;
						case "Tuesday":
							Title = cHomeTitle + " - Tuesday";
							_daySort = DayOfWeek.Tuesday;
							break;
						case "Wednesday":
							Title = cHomeTitle + " - Wednesday";
							_daySort = DayOfWeek.Wednesday;
							break;
						case "Thursday":
							Title = cHomeTitle + " - Thursday";
							_daySort = DayOfWeek.Thursday;
							break;
						case "Friday":
							Title = cHomeTitle + " - Friday";
							_daySort = DayOfWeek.Friday;
							break;
						case "Saturday":
							Title = cHomeTitle + " - Saturday";
							_daySort = DayOfWeek.Saturday;
							break;
					}
					#endregion

				var isDaySort = await SetByDay(_daySort);
				if (isDaySort) UserDialogs.Instance.HideLoading();
				break;
			}
		}

		/// <summary>
		/// Sets the list to the upcoming specials
		/// </summary>
		Task<bool> SetUpcoming()
		{
			var tcs = new TaskCompletionSource<bool>();
			_sortedSpecs = _allSpecs.Where(x => x.StartTime.HasValue
												&& x.StartTime > DateTime.Now.TimeOfDay
												&& x.DaysOfWeek.Contains(DateTime.Now.DayOfWeek))
				   					.OrderBy(x => x.StartTime).ToList();
			
			_listSpecs = _sortedSpecs.Take(offset);
			lvUpcomingSpecs.ItemsSource = new ObservableCollection<FoodSpecial>(_listSpecs);
			_specSort = SpecSort.Upcoming;
			tcs.SetResult(true);
			return tcs.Task;
		}

		/// <summary>
		/// Sets the list of specials by a particular day
		/// </summary>
		/// <param name="day">Day.</param>
		Task<bool> SetByDay(DayOfWeek day)
		{
			var tcs = new TaskCompletionSource<bool>();
			_sortedSpecs = _allSpecs.Where(x => x.DaysOfWeek.Contains(day));
			_listSpecs = _sortedSpecs.Take(offset);
			lvUpcomingSpecs.ItemsSource = new ObservableCollection<FoodSpecial>(_listSpecs);
			_specSort = SpecSort.ByDay;
			tcs.SetResult(true);
			return tcs.Task;
		}

		#endregion
	}
}
