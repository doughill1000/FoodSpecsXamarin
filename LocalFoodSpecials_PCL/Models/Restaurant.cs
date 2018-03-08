using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FoodSpecs.PCL
{
	public class Restaurant
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("image_url")]
		public string ImageUrl { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("price")]
		public string Price { get; set; }

		[JsonProperty("rating")]
		public double Rating { get; set; }

		[JsonProperty("review_count")]
		public int ReviewCount { get; set; }

		[JsonProperty("phone")]
		public string Phone { get; set; }

		[JsonProperty("photos")]
		public IList<string> Photos { get; set; }

		[JsonProperty("hours")]
		public IList<Hour> Hours { get; set; }

		[JsonProperty("categories")]
		public IList<Category> Categories { get; set; }

		[JsonProperty("coordinates")]
		public Coordinates Coordinates { get; set; }

		[JsonProperty("location")]
		public Location Location { get; set; }

		[JsonProperty("distance")]
		public decimal Distance { get; set;}

		public IList<FoodSpecial> FoodSpecials { get; set;}


		///// <summary>
		///// Gets or sets the restaurant identifier.
		///// </summary>
		///// <value>The restaurant identifier.</value>
		//public int RestaurantId { get; set; }

		///// <summary>
		///// Gets or sets the name of the restaurant.
		///// </summary>
		///// <value>The name of the restaurant.</value>
		//public string RestaurantName { get; set; }

		////public string City { get; set;}

		////public string State { get; set;}

		////public string Zip { get; set;}

		///// <summary>
		///// Gets or sets the restaurant address.
		///// </summary>
		///// <value>The restaurant address.</value>
		//public string FullAddress { get; set; }

		///// <summary>
		///// Gets or sets the map point for the restaurant which contains lat and long
		///// </summary>
		///// <value>The map point.</value>
		//public MapPoint MapPoint { get; set; }

		//public double Distance { get; set; }
	}
}

