using System;
using Newtonsoft.Json;
namespace FoodSpecs.PCL
{
	public class Review
	{
		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("user")]
		public User User { get; set; }

		[JsonProperty("rating")]
		public int Rating { get; set; }
	}
}
