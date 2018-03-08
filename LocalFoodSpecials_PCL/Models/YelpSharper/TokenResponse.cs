using System;
using Newtonsoft.Json;

namespace FoodSpecs.PCL
{
	public class TokenResponse : BaseResponse
	{
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }

		[JsonProperty("token_type")]
		public string TokenType { get; set; }

		[JsonProperty("expires_in")]
		public int ExpiresIn { get; set; }
	}
}
