using System;
using Newtonsoft.Json;

namespace FoodSpecs.PCL
{
	public class User
	{
		[JsonProperty("image_url")]
		public string ImageUrl { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}
}
