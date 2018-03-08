using Ninject.Modules;

namespace FoodSpecs.PCL
{
	public class Bindings : NinjectModule
	{
		public override void Load()
		{
			Bind<IRestaurantService>().To<RestaurantService>();
			Bind<IRestService>().To<RestService>();
			Bind<IFoodSpecialService>().To<FoodSpecialService>();
		}
	}
}

