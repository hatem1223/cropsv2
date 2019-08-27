using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CROPS.HttpClientUtility
{
    public class NullHttpClientManager : IHttpClientManager
    {
        public StringContent Response { get; set; }
        public HttpStatusCode ResultStatusCode { get; set; }


        public NullHttpClientManager()
        {
        }

        public async Task<HttpResponseMessage> PostAsync(Uri serviceUrl, StringContent content)
        {
            return new HttpResponseMessage(ResultStatusCode) { Content = Response };
        }

        public async Task<HttpResponseMessage> PutAsync(Uri serviceUrl, StringContent content)
        {
            return new HttpResponseMessage(ResultStatusCode) { Content = Response };
        }

        public async Task<HttpResponseMessage> DeleteAsync(Uri serviceUrl)
        {
            return new HttpResponseMessage(ResultStatusCode) { Content = Response };
        }

        public async Task<HttpResponseMessage> GetAsync(Uri serviceUrl)
        {
            return new HttpResponseMessage(ResultStatusCode) { Content = Response };
        }
    }
}
