using Newtonsoft.Json;

namespace FoodSpecs.PCL
{
	public class Coordinates
	{
		[JsonProperty("latitude")]
		public double Latitude { get; set; }

		[JsonProperty("longitude")]
		public double Longitude { get; set; }
	}
}