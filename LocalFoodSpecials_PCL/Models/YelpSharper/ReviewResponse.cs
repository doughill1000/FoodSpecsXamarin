using System.Collections.Generic;
using Newtonsoft.Json;

namespace FoodSpecs.PCL
{
	public class ReviewResponse : BaseResponse
	{
		[JsonProperty("reviews")]
		public IList<Review> Reviews { get; set; }

		[JsonProperty("total")]
		public int Total { get; set; }
	}
}
