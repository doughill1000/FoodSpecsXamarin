using System.Threading.Tasks;
using System.Net.Http;

namespace FoodSpecs.PCL
{
	public interface IRestService
	{
		/// <summary>
		/// Sends a get http request async
		/// </summary>
		/// <returns>The async object.</returns>
		/// <param name="url">the api url</param>
		Task<HttpResponseMessage> GetAsync(string url);

		/// <summary>
		/// Sends a put http request async. Covers add and update.
		/// </summary>
		/// <returns>The async object.</returns>
		/// <param name="url">URL.</param>
		/// <param name="json">Data being sent</param>
		Task<HttpResponseMessage> PostAsync(string url, string json);

		/// <summary>
		/// Sends a delete http request async
		/// </summary>
		/// <returns>If the request succeeded</returns>
		/// <param name="url">The api url</param>
		Task<bool> DeleteAsync(string url);
	}
}