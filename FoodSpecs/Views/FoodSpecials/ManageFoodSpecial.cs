using System;
using FoodSpecs.PCL;
using Ninject;
using Xamarin.Forms;
using System.Linq;
using Acr.UserDialogs;
using System.Threading.Tasks;

namespace FoodSpecs
{
	/// <summary>
	/// This page is used for managing food specials. 
	/// This includes: View, Edit, Add, Copy, Report.
	/// A buttload of code...
	/// </summary>
	public partial class ManageFoodSpecial : ContentPage
	{
		Restaurant _restaurant;
		FoodSpecial _spec;

		#region Constructor
		/// <summary>
		/// Constructor                       
		/// </summary>
		/// <param name="spec">Food Special.</param>
		public ManageFoodSpecial(FoodSpecial spec)
		{
			InitializeComponent();

			Title = spec.Title;
			_restaurant = spec.Restaurant;
			_spec = spec;

			NavigationPage.SetBackButtonTitle(this, "");
		}

		#endregion

		#region Overrides
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			await SetFields();
		}
		#endregion

		#region Private Methods

		/// <summary>
		/// Sets the values of the readonly fields
		/// </summary>
		async Task SetFields()
		{
			var spec = await FoodSpecialService.GetSpecsFromStorageAsync();
			_spec = spec.Single(x => x.SpecialId == _spec.SpecialId);
			RestaurantName.Text = _spec.Restaurant.Name;
			lblTitle.Text = _spec.Title;
			if(_spec.Description != null){
				lblDescription.Text = _spec.Description;
			}else{
				lblDescription.Text = "(No description)";
				lblDescription.TextColor = Color.Gray;
			}

			lblDays.Text = string.Join(", ", _spec.DaysOfWeek.Select(x => x.ToString())); //Comma deliminate days of week
			lblHours.Text = _spec.Hours;
		}

		async void ManageSpec(object sender, EventArgs e)
		{
			var action = await DisplayActionSheet("Actions", "Cancel", "Delete", new string[] { "Edit", "Copy", "Report" });
			switch (action)
			{
				#region Case Delete
				case "Delete":
					var confirm = await DisplayAlert("Delete", "Are you sure you want to delete " + _spec.Title + "?", "Yes", "No");
					if (confirm)
					{
						UserDialogs.Instance.ShowLoading("Deleting", MaskType.Black);
						IFoodSpecialService _foodSpecService = Startup.Container.Get<IFoodSpecialService>();
						await _foodSpecService.Delete(_spec.SpecialId);
						UserDialogs.Instance.HideLoading();
						await Navigation.PopAsync();
					}
					break;
				#endregion
				#region Case Edit

				case "Edit":
					var editFoodSpecPage = new AddEditFoodSpec(_restaurant, FoodSpecialActions.Edit, _spec);
					editFoodSpecPage.Disappearing += (send, args) =>
					{
					    SetFields();
					};

					await Navigation.PushModalAsync(editFoodSpecPage);
					break;
				#endregion
			}
		}
		#endregion
	}
}
