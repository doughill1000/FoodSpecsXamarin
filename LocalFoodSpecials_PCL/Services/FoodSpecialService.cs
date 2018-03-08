using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Text;
using Plugin.SecureStorage;

namespace FoodSpecs.PCL
{
	public class FoodSpecialService : IFoodSpecialService
	{
		readonly IRestService _restService;
		readonly IRestaurantService _restaurantService;

		#region Constructors
		public FoodSpecialService(IRestService _restService, IRestaurantService _restaurantService)
		{
			this._restService = _restService;
			this._restaurantService = _restaurantService;
		}
		#endregion

		#region Public Methods
		//<inheritdoc>
		public async Task<IEnumerable<FoodSpecial>> GetFoodSpecialsForRestaurant(string restaurantId)
		{
			var parameters = new Dictionary<string, string>();
			parameters.Add("restaurantId", restaurantId);
			var url = FormatFoodSpecialsUrl("/GetFoodSpecialsForRestaurant", parameters);

			var response = await _restService.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				return JsonConvert.DeserializeObject<IEnumerable<FoodSpecial>>(await response.Content.ReadAsStringAsync());
			}
			return null;
		}

		//<inheritdoc>
		public async Task AddUpdate(FoodSpecial foodSpec)
		{
			//Make web service
			await _restService.PostAsync(Constants.FoodSpecialsApiBaseUrl + "/Post", JsonConvert.SerializeObject(foodSpec));

			#region Update in local storage
			var restaurants = await RestaurantService.GetRestaurantsFromStorageAsync();
            foodSpec.Restaurant = restaurants.Single(x => x.Id == foodSpec.RestaurantId);
			var specs = await GetSpecsFromStorageAsync();
			if (foodSpec.SpecialId == 0)
			{
				specs.Add(foodSpec);
			}else{
				var old = specs.First(x => x.SpecialId == foodSpec.SpecialId);
				var index = specs.IndexOf(old);

				if (index != -1)
				{
					specs[index] = foodSpec;
				}
			}

			SetSpecsInStorage(specs);
			CrossSecureStorage.Current.SetValue(LSConstants.RefreshHome, "True");
			#endregion
		}

		//<inheritdoc>
		public async Task Delete(int id)
		{
			var parameters = new Dictionary<string, string>();
			parameters.Add("id", id.ToString());
			var url = FormatFoodSpecialsUrl("/Delete", parameters);

			var ok = await _restService.DeleteAsync(url);

			#region Update in local storage
			if (ok)
			{
				var specs = await GetSpecsFromStorageAsync();
				specs.Remove(specs.Single(x => x.SpecialId == id));
				SetSpecsInStorage(specs);
				CrossSecureStorage.Current.SetValue(LSConstants.RefreshHome, "True");
			}
			#endregion
		}

		//<inheritdoc>
		public async Task<IEnumerable<FoodSpecial>> GetFoodSpecsByCoordinates(){

			var storedFoodSpecsJson = CrossSecureStorage.Current.GetValue(LSConstants.Specs);
			if (storedFoodSpecsJson != null)
			{
				return JsonConvert.DeserializeObject<IEnumerable<FoodSpecial>>(storedFoodSpecsJson);
			}

			var restaurants = await _restaurantService.SearchRestaurantsByCoordinates();
			return await GetSpecsFromStorageAsync();
			//var foodSpecs = restaurants.SelectMany(x => x.FoodSpecials);
			//foreach(var spec in foodSpecs){
			//	spec.Restaurant = restaurants.Single(x => x.Id == spec.RestaurantId);
			//}
			//CrossSecureStorage.Current.SetValue(LSConstants.Specs, JsonConvert.SerializeObject(foodSpecs));

			//return foodSpecs; 
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Formats the food special URL.
		/// </summary>
		/// <returns>The food special URL.</returns>
		/// <param name="endpoint">Api endpoint</param>
		/// <param name="parameters">Dictionary of parameters</param>
		string FormatFoodSpecialsUrl(string endpoint, Dictionary<string, string> parameters)
		{
			return Utils.FormatUrl(Constants.FoodSpecialsApiBaseUrl + endpoint, parameters);
		}

		public static async Task<List<FoodSpecial>> GetSpecsFromStorageAsync(){
			return JsonConvert.DeserializeObject<IEnumerable<FoodSpecial>>(await Utils.GetValueLocalStorageAsync(LSConstants.Specs)).ToList();
		}

		public static void SetSpecsInStorage(List<FoodSpecial> specs){
			CrossSecureStorage.Current.SetValue(LSConstants.Specs, JsonConvert.SerializeObject(specs));
		}
		#endregion
	}
}

