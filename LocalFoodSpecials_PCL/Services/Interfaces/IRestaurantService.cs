using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodSpecs.PCL
{
	public interface IRestaurantService
	{
		/// <summary>
		/// Search restaurants by coodinates.
		/// </summary>
		/// <returns>List of restaurants</returns>
		Task<IEnumerable<Restaurant>> SearchRestaurantsByCoordinates(string term = "");

		/// <summary>
		/// Gets a list of restaurants by a partial name and location.
		/// </summary>
		/// <returns>The restaurants by partial name.</returns>
		/// <param name="location">String location where the search should happen (city, state)</param>
		/// <param name="term">A term for the restaurant (name)</param>
		Task<IEnumerable<Restaurant>> GetRestaurantsByLocationAndTerm(string term, string location);

		/// <summary>
		/// Gets a restaurant by yelp id.
		/// </summary>
		/// <returns>Restaurant</returns>
		/// <param name="id">Restaurant yelp id</param>
		Task<Restaurant> GetById(string id);

		/// <summary>
		/// Searches autocomplete for restaurants.
		/// </summary>
		/// <returns>Matching restaurants</returns>
		/// <param name="text">Keyword text</param>
		Task<IEnumerable<Restaurant>> SearchAutoComplete(string text);
	}
}