using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.SecureStorage;
using Newtonsoft.Json;

namespace FoodSpecs.PCL
{
	public static class Utils
	{
		//public static double GetDistance(double lat1, double long1, double lat2, double long2){
		//	double d = Math.Acos(
		//	   (Math.Sin(lat1) * Math.Sin(lat2)) +
		//	   (Math.Cos(lat1) * Math.Cos(lat2))
		//	   * Math.Cos(long2 - long1));

		//	//Convert meters to miles
		//	return 6378137 * d * 0.000621371;
		//}

		public static string FormatUrl(string endpoint, Dictionary<string, string> parameters)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(endpoint);
			if (parameters.Any())
			{
				sb.Append("?");
				var formattedParameters = new List<string>();
				foreach (KeyValuePair<string, string> kvp in parameters)
				{
					formattedParameters.Add(kvp.Key + "=" + kvp.Value);
				}
				sb.Append(string.Join("&", formattedParameters));
			}

			return sb.ToString();
		}

		//TODO look into set and get for lat and long

		public async static Task<Dictionary<string, string>> GetLatAndLong()
		{
			var latitude = string.Empty;
			var longitude = string.Empty;

			if (CrossSecureStorage.Current.HasKey(LSConstants.Latitude) || CrossSecureStorage.Current.HasKey(LSConstants.Longitude))
			{
				latitude = CrossSecureStorage.Current.GetValue(LSConstants.Latitude);
				longitude = CrossSecureStorage.Current.GetValue(LSConstants.Longitude);
			}
			else {
				var locator = CrossGeolocator.Current;
				locator.DesiredAccuracy = 500;
				var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000).ConfigureAwait(false);

				latitude = position.Latitude.ToString();
				longitude = position.Longitude.ToString();

				CrossSecureStorage.Current.SetValue(LSConstants.Latitude, latitude);
				CrossSecureStorage.Current.SetValue(LSConstants.Longitude, longitude);
			}

			return new Dictionary<string, string>{
				{"Latitude", latitude},
				{"Longitude", longitude}
			};
		}

		/// <summary>
		/// Checks if the user has permission to edit or delete a spec.
		/// </summary>
		/// <returns>If the user has permission</returns>
		/// <param name="id">uesr Id</param>
		public static bool hasPermissionForSpec(string id){
			var roles = JsonConvert.DeserializeObject<List<string>>(CrossSecureStorage.Current.GetValue(LSConstants.Roles));
			var userId = CrossSecureStorage.Current.GetValue(LSConstants.UserId);
			if(userId == id && roles.Contains("Admin")){
				return true;
			}
			return false;
		}

		public static void SetJsonSettings(){
			JsonConvert.DefaultSettings = () => new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};
		}

		public static Task<string> GetValueLocalStorageAsync(string key){
			var tcs = new TaskCompletionSource<string>();
			tcs.SetResult(CrossSecureStorage.Current.GetValue(key));
			return tcs.Task;
		}
	}
}

