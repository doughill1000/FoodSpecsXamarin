using System;

namespace FoodSpecs.PCL
{
	[FlagsAttribute]
	public enum FoodSpecialActions
		{
			View = 1,
			Add = 2,
			Edit = 4,
		}
}

