using System;
using FoodSpecs.PCL;
using Xamarin.Forms;

namespace FoodSpecs
{
	public partial class FoodSpecsPage : TabbedPage
	{
		public FoodSpecsPage()
		{
			InitializeComponent();
			NavigationPage.SetHasBackButton(this, false);
			NavigationPage.SetHasNavigationBar(this, false);

			//var overlay = new AbsoluteLayout();
			//var content = new StackLayout();
			//var loadingIndicator = new ActivityIndicator{
			//	IsRunning= true,
			//	IsVisible = true,
			//	Color = Color.Black
			//};
			//AbsoluteLayout.SetLayoutFlags(content, AbsoluteLayoutFlags.PositionProportional);
			//AbsoluteLayout.SetLayoutBounds(content, new Rectangle(0f, 0f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			//AbsoluteLayout.SetLayoutFlags(loadingIndicator, AbsoluteLayoutFlags.PositionProportional);
			//AbsoluteLayout.SetLayoutBounds(loadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			//overlay.Children.Add(content);
			//overlay.Children.Add(loadingIndicator);

			//var loadingPage = new ContentPage
			//{
			//	Content = overlay
			//};

			Children.Add(new NavigationPage(new Home()){
				Icon = "utensils.png",
				BarBackgroundColor = Color.FromHex(Constants.YelpRed),
				BarTextColor = Color.White
			});
			Children.Add(new NavigationPage(new Restaurants()){
				Icon = "restaurants.png",
				BarBackgroundColor = Color.FromHex(Constants.YelpRed),
				BarTextColor = Color.White
			});
			Children.Add(new NavigationPage(new Test_page()){
				Icon = "favorite.png",
				BarBackgroundColor = Color.FromHex(Constants.YelpRed),
				BarTextColor = Color.White,
			});

			//Activates when tab changes
			CurrentPageChanged += CurrentPageHasChanged;

			NavigationPage.SetBackButtonTitle(this, "");

			BarBackgroundColor = Color.FromHex(Constants.YelpRed);
			BarTextColor = Color.White;


		}

		/// <summary>
		/// Sets the title of the page to the title of the tabbed page. 
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected void CurrentPageHasChanged(object sender, EventArgs e)
		{
			var pageNumber = Children.IndexOf(CurrentPage);

			switch (pageNumber){
				case 0:
					Title = "Specs - Coming up";
					BarTextColor = Color.White;
					BarBackgroundColor = Color.FromHex(Constants.YelpRed);
					break;
				case 1:
					Title = "Restaurants";
					BarTextColor = Color.White;
					BarBackgroundColor = Color.FromHex(Constants.YelpRed);
					break;
				case 2:
					Title = "Search";
					BarTextColor = Color.White;
					BarBackgroundColor = Color.FromHex(Constants.YelpRed);
					break;
			}
		}
	}
}
