using System;
using System.Collections.Generic;

namespace FoodSpecs.PCL
{
	public class FoodSpecial
	{
		public FoodSpecial()
		{
		}

		/// <summary>
		/// Gets or sets the food special identifier.
		/// </summary>
		/// <value>The food special identifier.</value>
		public int SpecialId { get; set; }

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets whether special is on Sunday
		/// </summary>
		public bool Sunday { get; set; }

		/// <summary>
		/// Gets or sets a whether special is on Monday
		/// </summary>
		public bool Monday { get; set;}

		/// <summary>
		/// Gets or sets a whether special is on Tuesday
		/// </summary>
		public bool Tuesday { get; set; }

		/// <summary>
		/// Gets or sets a whether special is on Wednesday
		/// </summary>
		public bool Wednesday { get; set; }

		/// <summary>
		/// Gets or sets a whether special is on Thursday
		/// </summary>
		public bool Thursday { get; set; }

		/// <summary>
		/// Gets or sets a whether special is on Friday
		/// </summary>
		public bool Friday { get; set; }

		/// <summary>
		/// Gets or sets a whether special is on Saturday
		/// </summary>
		public bool Saturday { get; set; }

		/// <summary>
		/// Gets or sets the start time.
		/// </summary>
		/// <value>The start time.</value>
		public Nullable<TimeSpan> StartTime { get; set; }

		/// <summary>
		/// Gets or sets the end time.
		/// </summary>
		/// <value>The end time.</value>
		public Nullable<TimeSpan> EndTime { get; set; }

		/// <summary>
		/// Gets or sets whether teh food special is all day
		/// </summary>
		public bool AllDay { get; set;}

		/// <summary>
		/// Gets or sets the active.
		/// </summary>
		/// <value>The active.</value>
		public bool Active { get; set; }

		/// <summary>
		/// Id of the restaurant the special belongs to.
		/// </summary>
		/// <value>The restaurant identifier.</value>
		public string RestaurantId { get; set; }

		/// <summary>
		/// Id of the food special owner
		/// </summary>
		/// <value>The owner identifier.</value>
		public string OwnerId { get; set;}

		/// <summary>
		/// Gets or sets the restaurant the food special belongs to.
		/// </summary>
		/// <value>The restaurant.</value>
		public Restaurant Restaurant { get; set; }

		/// <summary>
		/// Gets or sets the day of week.
		/// </summary>
		/// <value>The day of week.</value>
		public IList<DayOfWeek> DaysOfWeek { get{
				var daysOfWeek = new List<DayOfWeek>();
				if (Sunday)
				{
					daysOfWeek.Add(DayOfWeek.Sunday);
				}
				if (Monday)
				{
					daysOfWeek.Add(DayOfWeek.Monday);
				}
				if (Tuesday)
				{
					daysOfWeek.Add(DayOfWeek.Tuesday);
				}
				if (Wednesday)
				{
					daysOfWeek.Add(DayOfWeek.Wednesday);
				}
				if (Thursday)
				{
					daysOfWeek.Add(DayOfWeek.Thursday);
				}
				if (Friday)
				{
					daysOfWeek.Add(DayOfWeek.Friday);
				}
				if (Saturday)
				{
					daysOfWeek.Add(DayOfWeek.Saturday);
				}
				return daysOfWeek;
			}
		}

		/// <summary>
		/// Formats the duration of the special using start and end time into a string 
		/// </summary>
		/// <value>The duration of the special.</value>
		public string Hours
		{
			get{
				if (StartTime.HasValue && EndTime.HasValue)
				{
					var startTime = DateTime.Today.Add(StartTime.Value).ToString("hh:mm tt");
					var endTime = DateTime.Today.Add(EndTime.Value).ToString("hh:mm tt");
					return startTime + " - " + endTime;
				}if(AllDay){
					return "All day";
				}
				return string.Empty;
			}
		}
	}
}

