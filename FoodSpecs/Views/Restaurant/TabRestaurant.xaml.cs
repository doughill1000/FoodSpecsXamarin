using System;
using System.Collections.Generic;
using Xamarin.Forms;
using FoodSpecs.PCL;

namespace FoodSpecs
{
	public partial class TabRestaurant : CarouselPage
	{
		public TabRestaurant(Restaurant restaurant)
		{
			InitializeComponent();

			Children.Add(new ViewRestaurantSpecs(restaurant));
			Children.Add(new RestaurantDetails(restaurant));

			//BarBackgroundColor = Color.FromHex(Constants.YelpRed);
			//BarTextColor = Color.White;
			Title = restaurant.Name;
			//NavigationPage.SetHasNavigationBar(this, false);
		}

		/// <summary>
		/// Sets the title of the page to the title of the tabbed page. 
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected void CurrentPageHasChanged(object sender, EventArgs e)
		{
			var pageNumber = Children.IndexOf(CurrentPage);

			switch (pageNumber)
			{
				case 0:
					Title = "Today's Specs";
					//BarTextColor = Color.White;
					//BarBackgroundColor = Color.FromHex(Constants.YelpRed);
					break;
				case 1:
					Title = "Details";
					ToolbarItems.Clear();
					break;
				//case 2:
				//	Title = "Search";
				//	//BarTextColor = Color.White;
				//	//BarBackgroundColor = Color.FromHex(Constants.YelpRed);
				//	break;
			}
		}
	}


}
