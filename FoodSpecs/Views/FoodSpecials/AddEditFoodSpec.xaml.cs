using Xamarin.Forms;
using FoodSpecs.PCL;
using System;
using Ninject;
using System.Threading.Tasks;
using Acr.UserDialogs;
using System.Text;

namespace FoodSpecs
{
	public partial class AddEditFoodSpec : ContentPage
	{
		FoodSpecial _spec;
		string _restaurantId;
		bool tapped;
		StringBuilder errorMessage = new StringBuilder();

		#region Constants
		const int maxTitleLength = 50;
		#endregion

		public AddEditFoodSpec(Restaurant restaurant, FoodSpecialActions action, FoodSpecial spec = null)
		{
			InitializeComponent();
			_spec = spec;
			_restaurantId = restaurant.Id;

			if (action == FoodSpecialActions.Add)
			{
				lblTitle.Text = "Add";
			}
			else if(action == FoodSpecialActions.Edit){
				lblTitle.Text = "Edit";
			}
				
			#region Tap Events
			var tgr = new TapGestureRecognizer();
			tgr.Tapped += async (s, e) => await Cancel();
			lblCancel.GestureRecognizers.Add(tgr);

			tgr = new TapGestureRecognizer();
			tgr.Tapped += async (s, e) => await AddUpdate();
			lblSave.GestureRecognizers.Add(tgr);
			#endregion

			if (action == FoodSpecialActions.Edit)
			{
				SetEditFields();
			}

			swAllDay.Toggled += ToggleVisibleTimes;
		}

		/// <summary>
		/// Saves thes food spec to the database.
		/// </summary>
		async Task AddUpdate()
		{
			//Prevent double tap.. we'll see how this works
			if(tapped){return;}
			tapped = true;

			//If validation fails, return
			if (ValidateForm() == false) {
				await DisplayAlert("Validation", errorMessage.ToString(), "OK"); 
				tapped = false;
				return;
			}

			UserDialogs.Instance.ShowLoading("Saving", MaskType.Black);

			IFoodSpecialService _foodSpecService = Startup.Container.Get<IFoodSpecialService>();

			#region Create new spec
			var newFoodSpec = new FoodSpecial
			{
				SpecialId = _spec != null ? _spec.SpecialId : 0, //new spec id is 0
				Title = txtTitle.Text,
				Description = txtDescription.Text,
				Sunday = swSunday.IsToggled,
				Monday = swMonday.IsToggled,
				Tuesday = swTuesday.IsToggled,
				Wednesday = swWednesday.IsToggled,
				Thursday = swThursday.IsToggled,
				Friday = swFriday.IsToggled,
				Saturday = swSaturday.IsToggled,
				RestaurantId = _restaurantId
			};
			#endregion

			//All day and set times are mutually exclusive

			if (swAllDay.IsToggled)
			{
				newFoodSpec.AllDay = true;
			}
			else {
				newFoodSpec.StartTime = tpStartTime.Time;
				newFoodSpec.EndTime = tpEndTime.Time;
			}

			try
			{
				await _foodSpecService.AddUpdate(newFoodSpec);
				UserDialogs.Instance.HideLoading();
				await Navigation.PopModalAsync(true);
				tapped = false;
			}
			catch (Exception ex)
			{
				UserDialogs.Instance.HideLoading();
				await DisplayAlert("Error", "Due to an error your food special has not been saved", "OK");
				tapped = false;
			}
		}

		void SetEditFields(){
			//Set fields to proper values
			txtTitle.Text = _spec.Title;
			txtDescription.Text = _spec.Description;
			swSunday.IsToggled = _spec.Sunday;
			swMonday.IsToggled = _spec.Monday;
			swTuesday.IsToggled = _spec.Tuesday;
			swWednesday.IsToggled = _spec.Wednesday;
			swThursday.IsToggled = _spec.Thursday;
			swFriday.IsToggled = _spec.Friday;
			swSaturday.IsToggled = _spec.Saturday;
			if (_spec.StartTime != null)
			{
				tpStartTime.Time = _spec.StartTime.Value;
			}
			if (_spec.EndTime != null)
			{
				tpEndTime.Time = _spec.EndTime.Value;
			}
			swAllDay.IsToggled = _spec.AllDay;

			ToggleVisibleTimes(new {}, new EventArgs());
		}

		/// <summary>
		/// Toggles the start and end time controls based on value of All Day
		/// </summary>
		void ToggleVisibleTimes(object sender, EventArgs e)
		{
			if (swAllDay.IsToggled)
			{
				TimeControls.IsVisible = false;
			}
			else {
				TimeControls.IsVisible = true;
			}
		}

		async Task Cancel(){
			if(tapped){return;}
			await Navigation.PopModalAsync();
		}

		#region Validation
		/// <summary>
		/// Performs validation on the form.
		/// </summary>
		/// <returns>If the form is valid.</returns>
		bool ValidateForm(){
			errorMessage.Clear();
			var validTitle = ValidateTitle();
			var validDays = ValidateDays();
			var validTimes = ValidateTimes();

			return validTitle && validDays && validTimes;
				
		}

		/// <summary>
		/// Validation for title field
		/// 1. Length of the title must greater than 0 and less than the max length.
		/// </summary>
		/// <returns>If the title is valid.</returns>
		bool ValidateTitle(){
			
			if(txtTitle.Text == null){
				errorMessage.AppendLine("Please enter a title for the spec");
				return false;
			}
			if(txtTitle.Text.Length > maxTitleLength){
				errorMessage.AppendLine("The maximum length for a spec title is 50 characters");
				return false;
			}
			return true;
		}

		/// <summary>
		/// Validates the days.
		/// 1. At least one day must be selected.
		/// </summary>
		/// <returns>If the days are valid.</returns>
		bool ValidateDays(){
			StringBuilder sb = new StringBuilder();
			//If no days are selected
			if(!swSunday.IsToggled &&
			   !swMonday.IsToggled &&
			   !swTuesday.IsToggled &&
			   !swWednesday.IsToggled &&
			   !swThursday.IsToggled &&
			   !swFriday.IsToggled &&
			   !swSaturday.IsToggled){
				errorMessage.AppendLine("Please select at least one day");
				return false;
			}
			return true;
		}

		bool ValidateTimes(){
			if(tpStartTime.Time == tpEndTime.Time){
				errorMessage.AppendLine("Start and end time cannot be the same");
				return false;
			}
			return true;
		}

		#endregion

	}
}
