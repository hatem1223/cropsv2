using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CROPS.HttpClientUtility
{
    public interface IHttpClientManager
    {
        Task<HttpResponseMessage> PostAsync(Uri serviceUrl, StringContent content);

        Task<HttpResponseMessage> PutAsync(Uri serviceUrl, StringContent content);

        Task<HttpResponseMessage> DeleteAsync(Uri serviceUrl);

        Task<HttpResponseMessage> GetAsync(Uri serviceUrl);
    }
}
