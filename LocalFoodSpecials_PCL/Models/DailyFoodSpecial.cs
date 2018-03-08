using System;
using System.Diagnostics.Contracts;
namespace FoodSpecs.PCL
{
	public class DailyFoodSpecial : FoodSpecial
	{
		public DailyFoodSpecial()
		{
			
		}

		/// <summary>
		/// Gets or sets if food special is active on Sunday
		/// </summary>
		/// <value>Is daily food special active on Sunday</value>
		public bool Sunday { get; set; }

		/// <summary>
		/// Gets or sets if food special is active on Monday
		/// </summary>
		/// <value>Is daily food special active on Monday</value>
		public bool Monday { get; set; }

		/// <summary>
		/// Gets or sets if food special is active on Tuesday
		/// </summary>
		/// <value>Is daily food special active on Tuesday</value>
		public bool Tuesday { get; set; }

		/// <summary>
		/// Gets or sets if food special is active on Wednesday
		/// </summary>
		/// <value>Is daily food special active on Wednesday</value>
		public bool Wednesday { get; set; }

		/// <summary>
		/// Gets or sets if food special is active on Thursday
		/// </summary>
		/// <value>Is daily food special active on Thursday</value>
		public bool Thursday { get; set; }

		/// <summary>
		/// Gets or sets if food special is active on Friday
		/// </summary>
		/// <value>Is daily food special active on Friday</value>
		public bool Friday { get; set; }

		/// <summary>
		/// Gets or sets if food special is active on Saturday
		/// </summary>
		/// <value>Is daily food special active on Saturday</value>
		public bool Saturday { get; set; }

	}
}

