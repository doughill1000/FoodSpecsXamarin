using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using FoodSpecs.PCL;
using Newtonsoft.Json;
using Plugin.SecureStorage;

namespace FoodSpecs.PCL
{
	public class YelpSharperClient
	{
		#region Constants
		private const string ApiUrl = "https://api.yelp.com/";
		private const string ApiVersion = "v3";
		private const string AuthPath = "oauth2/token";
		#endregion

		#region Properties
		public string AccessToken { get; set; }
		#endregion

		#region Constructors
		public YelpSharperClient() { }

		public YelpSharperClient(string accessToken)
		{
			AccessToken = accessToken;
		}
		#endregion

		#region Instance Yelp Api Methods
		public Task<TokenResponse> GetAsyncToken(string clientId, string clientSecret)
		{
			var client = GetHttpClient();
			var tcs = new TaskCompletionSource<TokenResponse>();

			var responseTask = client.PostAsync(AuthPath, new FormUrlEncodedContent(new[]
			{
					new KeyValuePair<string, string>("grant_type", "client_credentials"),
					new KeyValuePair<string, string>("client_id", clientId),
					new KeyValuePair<string, string>("client_secret", clientSecret)
				}));
			ResponseHandle(responseTask, tcs);
			DisposeHttpClient(tcs, client);
			return tcs.Task;
		}

		public TokenResponse GetToken(string clientId, string clientSecret)
		{
			var token =  GetAsyncToken(clientId, clientSecret).Result;
			//CUSTOM FOODSPECS
			CrossSecureStorage.Current.SetValue(LSConstants.YelpToken, token.AccessToken);
			return token;
		}

		public Task<SearchResponse> SearchAsync(object queryParams)
		{
			return GetAsync<SearchResponse>("/businesses/search", queryParams);
		}

		public Task<SearchResponse> SearchByPhoneAsync(object queryParams)
		{
			return GetAsync<SearchResponse>("/businesses/search/phone", queryParams);
		}

		public Task<SearchResponse> SearchByTransactionAsync(string transactionType, object queryParams)
		{
			return GetAsync<SearchResponse>($"/transactions/{transactionType}/search", queryParams);
		}

		public Task<Restaurant> GetRestaurantAsync(string id)
		{
			return GetAsync<Restaurant>($"/businesses/{id}");
		}

		public Task<ReviewResponse> GetRestaurantReviewsAsync(string id)
		{
			return GetAsync<ReviewResponse>($"/businesses/{id}/reviews");
		}

		public Task<AutoCompleteResponse> AutocompleteAsync(object queryParams)
		{
			return GetAsync<AutoCompleteResponse>("/autocomplete", queryParams);
		}

		public SearchResponse Search(object queryParams)
		{
			return Get<SearchResponse>("/businesses/search", queryParams);
		}

		public SearchResponse SearchByPhone(object queryParams)
		{
			return Get<SearchResponse>("/businesses/search/phone", queryParams);
		}

		public SearchResponse SearchByTransaction(string transactionType, object queryParams)
		{
			return Get<SearchResponse>($"/transactions/{transactionType}/search", queryParams);
		}

		public Restaurant GetRestaurant(string id)
		{
			return Get<Restaurant>($"/businesses/{id}");
		}

		public ReviewResponse GetRestaurantReviews(string id)
		{
			return Get<ReviewResponse>($"/businesses/{id}/reviews");
		}

		public AutoCompleteResponse Autocomplete(object queryParams)
		{
			return Get<AutoCompleteResponse>("/autocomplete", queryParams);
		}
		#endregion

		#region Instance Methods
		public Task<T> GetAsync<T>(string endPoint, object queryParams = null)
		{
			if (string.IsNullOrEmpty(AccessToken))
				throw new Exception("AccessToken should not be null or empty");

			endPoint = string.Concat(ApiVersion, AppendQueryParameters(endPoint, queryParams));
			var client = GetHttpClient();

			var tcs = new TaskCompletionSource<T>();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
			var responseTask = client.GetAsync(endPoint);
			ResponseHandle(responseTask, tcs);
			DisposeHttpClient(tcs, client);
			return tcs.Task;
		}

		public T Get<T>(string endPoint, object queryParams = null)
		{
			return GetAsync<T>(endPoint, queryParams).Result;
		}
		#endregion

		#region Private Methods
		private static string AppendQueryParameters(string endPoint, object parameters)
		{
			if (parameters == null)
			{
				return endPoint;
			}
			var dictionary = parameters as IDictionary<string, object>;
			var parms = string.Empty;
			var type = parameters.GetType();
			if (dictionary == null)
			{
				if (type.Name != "String")
				{
					var queryParams =
						type
							.GetRuntimeProperties()
							.Where(x => x.CanRead)
							.Select(x => x.Name + "=" + Uri.EscapeUriString(Convert.ToString(x.GetValue(parameters, null)))).ToArray();

					parms = queryParams.Length > 0 ? string.Join("&", queryParams) : parameters.ToString();
				}
				else
				{
					parms = Uri.EscapeUriString(parameters.ToString());
				}
			}
			else
			{
				var queryParams = dictionary.Select(x => x.Key + "=" + Uri.EscapeUriString(Convert.ToString(x.Value))).ToArray();
				if (queryParams.Length > 0)
					parms = string.Join("&", queryParams);
			}
			if (endPoint.Contains("?"))
			{
				if (endPoint.EndsWith("?") || endPoint.EndsWith("&"))
					endPoint = string.Concat(endPoint, parms);
				else
					endPoint = string.Concat(endPoint, "&", parms);
			}
			else
			{
				endPoint = string.Concat(endPoint, "?", parms);
			}
			return endPoint;
		}

		private static HttpClient GetHttpClient()
		{
			var client = new HttpClient { BaseAddress = new Uri(ApiUrl) };
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			return client;
		}

		private static void ResponseHandle<T>(Task<HttpResponseMessage> responseTask, TaskCompletionSource<T> tcs)
		{
			responseTask.ContinueWith((responseAction) =>
			{
				var response = responseAction.Result;
				if (response.StatusCode == HttpStatusCode.NotFound)
				{
					tcs.SetResult(default(T));
				}
				response.Content.ReadAsStringAsync()
					.ContinueWith((content) =>
					{
						try
						{
							tcs.SetResult(JsonConvert.DeserializeObject<T>(content.Result));
						}
						catch (Exception ex)
						{
							tcs.SetException(ex);
						}

					});
			});
		}

		private static void DisposeHttpClient<T>(TaskCompletionSource<T> tcs, HttpClient client)
		{
			if (tcs.Task.IsCompleted)
			{
				client.Dispose();
			}
		}
		#endregion
	}
}