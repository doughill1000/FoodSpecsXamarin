using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using Plugin.SecureStorage;

namespace FoodSpecs.PCL
{
	public class RestaurantService : IRestaurantService
	{
        const string CATEGORIES = "restaurants,nightlife";

		#region Constructor
		/// <summary>
		/// The rest service.
		/// </summary>
		readonly IRestService _restService;
		readonly YelpSharperClient _yelpClient;

		public RestaurantService(IRestService _restService) {
			this._restService = _restService;

			//Set up yelp client, use stored token when possible
			_yelpClient = new YelpSharperClient();
			var storedYelpToken = _yelpClient.AccessToken = CrossSecureStorage.Current.GetValue(LSConstants.YelpToken);
			if(storedYelpToken != null){
				_yelpClient.AccessToken = storedYelpToken;
			}else{
				var token = _yelpClient.GetToken(Constants.YelpAppId, Constants.YelpAppSecret);
				_yelpClient.AccessToken = token.AccessToken;
			}

		}
		#endregion

		#region Public Methods

		//<inheritdoc>
		public async Task<IEnumerable<Restaurant>> SearchRestaurantsByCoordinates(string term = "")
		{
			//Get coordinate, will check for cached if there
			var coordinates = await Utils.GetLatAndLong().ConfigureAwait(false);

			//Format api url to get restaurants
			var parameters = new Dictionary<string, string>();
			parameters.Add("latitude", coordinates["Latitude"]);
			parameters.Add("longitude", coordinates["Longitude"]);
            parameters.Add("term", term);
			var url = FormatRestaurantUrl("/SearchRestaurantsByCoordinates", parameters);

			var response = await _restService.PostAsync(url, null).ConfigureAwait(false);
			if (response.IsSuccessStatusCode)
			{
				var jsonRestaurants = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
				//Set in local storage
				CrossSecureStorage.Current.SetValue(LSConstants.Restaurants, jsonRestaurants);
				var restaurants = JsonConvert.DeserializeObject<IEnumerable<Restaurant>>(jsonRestaurants);
				var foodSpecs = restaurants.SelectMany(x => x.FoodSpecials);
				foreach (var spec in foodSpecs)
				{
					spec.Restaurant = restaurants.Single(x => x.Id == spec.RestaurantId);
				}
				CrossSecureStorage.Current.SetValue(LSConstants.Specs, JsonConvert.SerializeObject(foodSpecs));

				return restaurants;
			}
			return null;
		}		

		//<inheritdoc>
		public async Task<IEnumerable<Restaurant>> GetRestaurantsByLocationAndTerm(string term, string location)
		{
			var searchParams = new object();
			if (string.IsNullOrEmpty(location))
			{
				var coordinates = await Utils.GetLatAndLong().ConfigureAwait(false);
				searchParams = (new
				{
					term,
					latitude = coordinates["Latitude"],
					longitude = coordinates["Longitude"],
					limit = "50",
					radius = "40000",
					//categories = CATEGORIES
				});
			}
			else {
				searchParams = (new
				{
					term,
					location,
					limit = "50",
					radius = "40000",
					categories = CATEGORIES
				});
			}

			var result = await _yelpClient.SearchAsync(searchParams).ConfigureAwait(false);

			return result.Restaurants;
		}

		//<inheritdoc>
		public async Task<Restaurant> GetById(string id){

			return await _yelpClient.GetRestaurantAsync(id).ConfigureAwait(false); 
		}

		//<inheritdoc>
		public async Task<IEnumerable<Restaurant>> SearchAutoComplete(string text)
		{
			var coordinates = await Utils.GetLatAndLong().ConfigureAwait(false);

			var result = await _yelpClient.AutocompleteAsync(new
			{
				text,
				latitude = coordinates["Latitude"],
				longitude = coordinates["Longitude"]
			});

			return result.Restaurants;
		}

        public static async Task<List<Restaurant>> GetRestaurantsFromStorageAsync()
        {
            return JsonConvert.DeserializeObject<List<Restaurant>>(await Utils.GetValueLocalStorageAsync(LSConstants.Restaurants).ConfigureAwait(false));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the restaurants
        /// </summary>
        /// <returns>The restaurants</returns>
        /// <param name="url">The url endpoint</param>
        async Task<IEnumerable<Restaurant>> GetRestaurants(string url)
		{
			var response = await _restService.GetAsync(url);
			if(response.IsSuccessStatusCode){
				return JsonConvert.DeserializeObject<IEnumerable<Restaurant>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
			}

			return null;
		}

		/// <summary>
		/// Gets the restaurant
		/// </summary>
		/// <returns>The restaurants</returns>
		/// <param name="url">The url endpoint</param>
		async Task<Restaurant> GetRestaurant(string url)
		{
			var response = await _restService.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				return JsonConvert.DeserializeObject<Restaurant>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
			}
			return null;
		}

		/// <summary>
		/// Formats the restaurant URL. Defaults in restaurant api string.
		/// </summary>
		/// <returns>The restaurant URL.</returns>
		/// <param name="endpoint">Api endpoint</param>
		/// <param name="parameters">Dictionary of parameters</param>
		private string FormatRestaurantUrl(string endpoint, Dictionary<string, string> parameters){
			
			return Utils.FormatUrl(Constants.RestaurantApiBaseUrl + endpoint, parameters);
		}

		#endregion
	}
}

	