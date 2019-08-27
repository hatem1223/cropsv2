using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CROPS.HttpClientUtility
{
    public class HttpClientManager : IHttpClientManager
    {

        private readonly HttpClient _client;

        public HttpClientManager(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> PostAsync(Uri serviceUrl, StringContent content)
        {
            using (var client = _client ?? new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync(serviceUrl, content);
                return response;
            }
        }

        public async Task<HttpResponseMessage> PutAsync(Uri serviceUrl, StringContent content)
        {
            using (var client = _client ?? new HttpClient())
            {
                HttpResponseMessage response = await client.PutAsync(serviceUrl, content);
                return response;
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(Uri serviceUrl)
        {
            using (var client = _client ?? new HttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync(serviceUrl);
                return response;
            }
        }

        public async Task<HttpResponseMessage> GetAsync(Uri serviceUrl)
        {
            using (var client = _client ?? new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(serviceUrl);
                return response;
            }
        }
    }
}
