using System.Collections.Generic;
using Newtonsoft.Json;

namespace FoodSpecs.PCL
{
	public class SearchResponse : BaseResponse
	{
		[JsonProperty("total")]
		public int Total { get; set; }

		[JsonProperty("businesses")]
		public IList<Restaurant> Restaurants { get; set; }
	}
}

