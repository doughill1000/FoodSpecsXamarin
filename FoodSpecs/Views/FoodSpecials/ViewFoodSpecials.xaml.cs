using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FoodSpecs.PCL;
using Ninject;
using Xamarin.Forms;

namespace FoodSpecs
{
	public partial class ViewFoodSpecials : ContentPage
	{
		public ViewFoodSpecials(string restaurantId)
		{
			InitializeComponent();

			IDailyFoodSpecialService _dailyFoodSpecialService = Startup.Container.Get<IDailyFoodSpecialService>();

			lvDailyFoodSpecials.ItemsSource = new ObservableCollection<DailyFoodSpecial>(_dailyFoodSpecialService.GetDailyFoodSpecialsForRestaurant(restaurantId));
		}
	}
}
