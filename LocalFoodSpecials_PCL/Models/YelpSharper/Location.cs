using System.Text;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FoodSpecs.PCL
{
	public class Location
	{
		[JsonProperty("city")]
		public string City { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("address2")]
		public string Address2 { get; set; }

		[JsonProperty("address3")]
		public string Address3 { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("address1")]
		public string Address1 { get; set; }

		[JsonProperty("zip_code")]
		public string ZipCode { get; set; }

		public string DisplayAddress
		{
			get
			{
				var firstLine = new List<string> { Address1, Address2, Address3, City };
				var sb = new StringBuilder();
				sb.Append(string.Join(" ", firstLine.Where(x => !string.IsNullOrEmpty(x))));
				sb.Append(", " + State + " " + ZipCode);

				return sb.ToString();
			}
		}
	}
}
