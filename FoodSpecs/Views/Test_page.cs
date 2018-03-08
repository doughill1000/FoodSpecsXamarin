using System;

using Xamarin.Forms;

namespace FoodSpecs
{
	public class Test_page : ContentPage
	{
		public Test_page()
		{
			Icon = "favorite.png";
			Title = "Favorites";
			Content = new StackLayout
			{
				Children = {
					new Label { Text = "Coming soon.." }
				}
			};
		}
	}
}