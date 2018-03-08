using System.Collections.Generic;
using Newtonsoft.Json;

namespace FoodSpecs.PCL
{
	public class Hour
	{
		[JsonProperty("hours_type")]
		public string HoursType { get; set; }

		[JsonProperty("open")]
		public IList<Open> Open { get; set; }

		[JsonProperty("is_open_now")]
		public bool IsOpenNow { get; set; }
	}
}
