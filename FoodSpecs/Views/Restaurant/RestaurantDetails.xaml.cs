using System;
using System.Collections.Generic;
using FoodSpecs.PCL;
using Ninject;
using Xamarin.Forms;

namespace FoodSpecs
{
	public partial class RestaurantDetails : ContentPage
	{
		Restaurant _restaurant;
		public RestaurantDetails(Restaurant restaurant)
		{
			InitializeComponent();

			Device.BeginInvokeOnMainThread(async () =>
			{
				IRestaurantService _restuarantService = Startup.Container.Get<IRestaurantService>();
				var details = await _restuarantService.GetById(restaurant.Id);
				lblTest.Text = "Review Count:" + details.ReviewCount;
			});

			Title = "Details";
		}
	}
}
