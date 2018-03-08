using Newtonsoft.Json;

namespace FoodSpecs.PCL
{
	public class BaseResponse
	{
		[JsonProperty("error")]
		public Error Error { get; set; }
	}
}
