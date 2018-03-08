using System;
using System.Collections.Generic;
using FoodSpecs.PCL;
using Ninject;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FoodSpecs
{
	public partial class ViewRestaurantSpecs : ContentPage
	{
		List<FoodSpecial> _foodSpecials;
		Restaurant _restaurant;
		bool tapped;

		public ViewRestaurantSpecs(Restaurant restaurant)
		{
			this.InitializeComponent();
			Title = restaurant.Name;
			NavigationPage.SetBackButtonTitle(this, "");

			_restaurant = restaurant;

			Device.BeginInvokeOnMainThread(() =>
			{
				
			});

			Loading.IsVisible = false;
			Loading.IsRunning = false;
			SpecsPanel.IsVisible = true;

			lvFoodSpecials.ItemTapped += async (sender, e) =>
			{
				((ListView)sender).SelectedItem = null;
				var flattenedSpec = (FoodSpecial)e.Item;
				var spec = await FoodSpecialService.GetSpecsFromStorageAsync();

				await Navigation.PushAsync(new ManageFoodSpecial(spec.Single(x => x.SpecialId == flattenedSpec.SpecialId)));
			};
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			Device.BeginInvokeOnMainThread(async () => { await PopulateList(_restaurant);});
		}

		async Task PopulateList(Restaurant restaurant){
			//IRestaurantService _restuarantService = Startup.Container.Get<IRestaurantService>();
			//IFoodSpecialService _foodSpecService = Startup.Container.Get<IFoodSpecialService>();

			//var foodSpecials = await _foodSpecService.GetFoodSpecialsForRestaurant(restaurant.Id);
			//_foodSpecials = foodSpecials.ToList();

			var specs = await FoodSpecialService.GetSpecsFromStorageAsync();
			var foodSpecialsFlat = GetFlattenedFoodSpecials(specs.Where(x => x.RestaurantId == restaurant.Id));

			var groupings = foodSpecialsFlat
				.GroupBy(fs => fs.DaysOfWeek.First())
				.OrderBy(g => g.Key)
				.Select(x => new Grouping<DayOfWeek, FoodSpecial>(x.Key, x));

			lvFoodSpecials.ItemsSource = new ObservableCollection<Grouping<DayOfWeek, FoodSpecial>>(groupings);

			lvFoodSpecials.IsGroupingEnabled = true;
		}

		/// <summary>
		/// Opens the page to add a food special for a restaurant.
		/// </summary>
		async void AddFoodSpec(object sender, EventArgs e){
			if(tapped){
				return;
			}
			tapped = true;
			var addFoodSpecialPage = new AddEditFoodSpec(_restaurant, FoodSpecialActions.Add);
			addFoodSpecialPage.Disappearing += async (send, args) =>
			{
				await PopulateList(_restaurant);
			};
			tapped = false;
			await Navigation.PushModalAsync(addFoodSpecialPage);
		}

		void Edit(object sender, EventArgs e){
			
		}

		async void Delete(object sender, EventArgs e){
			
			IFoodSpecialService _foodSpecService = Startup.Container.Get<IFoodSpecialService>();

			var spec = (sender as MenuItem).BindingContext as FoodSpecial;

			await _foodSpecService.Delete(spec.SpecialId);
			_foodSpecials.Remove(spec);
		}

		#region Private Methods
		List<FoodSpecial> GetFlattenedFoodSpecials(IEnumerable<FoodSpecial> foodSpecials){
			
			var foodSpecialsFlat = new List<FoodSpecial>();

			//Flatten food specials so that there are multiple food specials created if food special occurs on multiple days.
			foreach (var spec in foodSpecials)
			{
				foreach (var dayOfWeek in spec.DaysOfWeek)
				{
					var newSpec = new FoodSpecial
					{
						SpecialId = spec.SpecialId,
						Title = spec.Title,
						StartTime = spec.StartTime,
						EndTime = spec.EndTime,
						AllDay = spec.AllDay, 
						OwnerId = spec.OwnerId,
						RestaurantId = spec.RestaurantId
					};

					#region Day of week case statement

					switch(dayOfWeek){
						case DayOfWeek.Sunday:
							newSpec.Sunday = true;
							break;
						case DayOfWeek.Monday:
							newSpec.Monday = true;
							break;
						case DayOfWeek.Tuesday:
							newSpec.Tuesday = true;
							break;
						case DayOfWeek.Wednesday:
							newSpec.Wednesday = true;
							break;
						case DayOfWeek.Thursday:
							newSpec.Thursday = true;
							break;
						case DayOfWeek.Friday:
							newSpec.Friday = true;
							break;
						case DayOfWeek.Saturday:
							newSpec.Saturday = true;
							break;
					}

					#endregion

					foodSpecialsFlat.Add(newSpec);
				}
			}

			return foodSpecialsFlat.ToList();
		}
		#endregion
	}
}
