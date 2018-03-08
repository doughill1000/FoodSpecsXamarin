using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
namespace FoodSpecs.PCL
{
	#region Interface
	[ContractClass(typeof(IFoodSpecialService))]
	public interface IFoodSpecialService
	{
		/// <summary>
		/// Get the food specials for a resaurant
		/// </summary>
		/// <returns>The food specials for restaurant.</returns>
		/// <param name="restaurant_Id">Restaurant id</param>
		Task<IEnumerable<FoodSpecial>> GetFoodSpecialsForRestaurant(string restaurant_Id);

		/// <summary>
		/// Add a food special
		/// </summary>
		/// <param name="foodSpec">New food special</param>
		/// <param name="restaurantId">Restaurant Id</param>
		Task AddUpdate(FoodSpecial foodSpec);

		/// <summary>
		/// Deletes a food special.
		/// </summary>
		/// <param name="id">Identifier.</param>
		Task Delete(int id);

		/// <summary>
		/// Gets the food specs from local storage. 
		/// </summary>
		/// <returns>The food specs from local storage.</returns>
		Task<IEnumerable<FoodSpecial>> GetFoodSpecsByCoordinates();
	}
	#endregion

	#region Contract Class
	[ContractClassFor(typeof(IFoodSpecialService))]
	abstract class IFoodSpecialServiceContract : IFoodSpecialService
	{
		public Task<IEnumerable<FoodSpecial>> GetFoodSpecialsForRestaurant(string restaurantId)
		{
			Contract.Requires(!string.IsNullOrEmpty(restaurantId));
			throw new NotImplementedException();
		}

		public Task AddUpdate(FoodSpecial foodSpec){
			Contract.Requires(foodSpec != null);
			throw new NotImplementedException();
		}

		public Task Delete(int id){
			Contract.Requires(id > 0);
			throw new NotImplementedException();
		}

		public Task<IEnumerable<FoodSpecial>> GetFoodSpecsByCoordinates(){
			throw new NotImplementedException();
		}
	}
	#endregion
}

