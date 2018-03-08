using Newtonsoft.Json;

namespace FoodSpecs.PCL
{
	public class Category
	{
		[JsonProperty("alias")]
		public string Alias { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }
	}
}
