using System.Collections.Generic;
using Newtonsoft.Json;

namespace FoodSpecs.PCL
{
	public class AutoCompleteResponse : BaseResponse
	{
		[JsonProperty("terms")]
		public IList<Term> Terms { get; set; }

		[JsonProperty("businesses")]
		public IList<Restaurant> Restaurants { get; set; }

		[JsonProperty("categories")]
		public IList<Category> Categories { get; set; }
	}
}
