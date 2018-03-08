using System;
using Ninject;
using FoodSpecs.PCL;

namespace FoodSpecs.PCL
{
	public static class Startup
	{
		public static StandardKernel Container { get; set; }

		public static void BuildContainer()
		{
			var kernel = new StandardKernel(new Bindings());
			Startup.Container = kernel;
		}
	}
}

