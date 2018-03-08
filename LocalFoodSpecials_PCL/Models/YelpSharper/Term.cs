using Newtonsoft.Json;
namespace FoodSpecs.PCL
{
	public class Term
	{
		[JsonProperty("text")]
		public string Text { get; set; }
	}
}
