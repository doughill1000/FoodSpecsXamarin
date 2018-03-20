using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Plugin.SecureStorage;
using System.Net;
using System.Text;

namespace FoodSpecs.PCL
{
    /// <summary>
    /// Contains the needed rest protocol for interacting with the Web API
    /// </summary>
    public class RestService : IRestService
    {
        readonly HttpClient httpClient;

        #region Constructor
        /// <summary>
        /// Creates a new instance of the rest service class
        /// </summary>
        public RestService()
        {
            httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(0, 0, 15); //15 second timeout
        }
        #endregion

        #region Public Methods
        //<inheritdoc>
        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var uri = new Uri(Constants.BaseUrl + url);

            var token = await GetAPIToken().ConfigureAwait(false);

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = new HttpResponseMessage();
            response = await httpClient.GetAsync(uri).ConfigureAwait(false);

            return response;
        }

        //<inheritdoc>
        public async Task<HttpResponseMessage> PostAsync(string url, string json)
        {
            var uri = new Uri(Constants.BaseUrl + url);

            var token = await GetAPIToken().ConfigureAwait(false);

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = json != null ? new StringContent(json, Encoding.UTF8, "application/json") : null;
            var response = await httpClient.PostAsync(uri, content).ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }

            return response;
        }

        //<inheritdoc>
        public async Task<bool> DeleteAsync(string url)
        {
            var uri = new Uri(Constants.BaseUrl + url);

            var token = await GetAPIToken().ConfigureAwait(false);

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.DeleteAsync(uri).ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
            return true;
        }


        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the API Token for the specs rest service and stores in local storage.
        /// </summary>
        /// <returns>The API token.</returns>
        async Task<string> GetAPIToken()
        {
            var storedToken = CrossSecureStorage.Current.GetValue(LSConstants.AccessToken);
            if (storedToken != null)
            {
                return storedToken;
            }

            // values = 
            var values = new List<KeyValuePair<string, string>>();

            values.Add(new KeyValuePair<string, string>("grant_type", "password"));
            values.Add(new KeyValuePair<string, string>("username", CrossSecureStorage.Current.GetValue(LSConstants.UserName)));
            values.Add(new KeyValuePair<string, string>("password", CrossSecureStorage.Current.GetValue(LSConstants.Password)));

            var formContent = new FormUrlEncodedContent(values);

            var responseMessage = await httpClient.PostAsync(Constants.BaseUrl + "/Token", formContent).ConfigureAwait(false);

            try
            {
                var responseJson = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                var token = JObject.Parse(responseJson);
                var accessToken = token.GetValue("access_token").ToString();
                CrossSecureStorage.Current.SetValue(LSConstants.AccessToken, accessToken);
                CrossSecureStorage.Current.SetValue(LSConstants.UserId, token.GetValue("UserId").ToString());
                CrossSecureStorage.Current.SetValue(LSConstants.Roles, token.GetValue("Roles").ToString());
                return accessToken;
            }
            catch (Exception ex)
            {
                throw new HttpRequestException();
            }
        }
        #endregion
    }
}

