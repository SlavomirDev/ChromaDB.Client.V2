using Newtonsoft.Json;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ChromaDB.Client.V2.Http
{
    internal static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient httpClient, string uri, object payload = null, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken cancellationToken = default)
        {
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri).WithJsonContent(payload, jsonSerializerSettings);
            return await SendAsync(httpClient, httpRequestMessage, cancellationToken);
        }
        public static async Task<HttpResponseMessage> PutAsJsonAsync(this HttpClient httpClient, string uri, object payload = null, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken cancellationToken = default)
        {
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, uri).WithJsonContent(payload, jsonSerializerSettings);
            return await SendAsync(httpClient, httpRequestMessage, cancellationToken);
        }
        public static async Task<HttpResponseMessage> PatchAsJsonAsync(this HttpClient httpClient, string uri, object payload = null, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken cancellationToken = default)
        {
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri).WithJsonContent(payload, jsonSerializerSettings);
            return await SendAsync(httpClient, httpRequestMessage, cancellationToken);
        }
        public static async Task<HttpResponseMessage> DeleteAsJsonAsync(this HttpClient httpClient, string uri, object payload = null, JsonSerializerSettings jsonSerializerSettings = null, CancellationToken cancellationToken = default)
        {
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri).WithJsonContent(payload, jsonSerializerSettings);
            return await SendAsync(httpClient, httpRequestMessage, cancellationToken);
        }

        internal static HttpRequestMessage WithJsonContent(this HttpRequestMessage httpRequestMessage, object payload = null, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (payload != null)
                httpRequestMessage.Content = new JsonContent(payload, jsonSerializerSettings);

            return httpRequestMessage;
        }

        private static Task<HttpResponseMessage> SendAsync(HttpClient httpClient, HttpRequestMessage requestMessage, CancellationToken cancellationToken = default)
            => httpClient.SendAsync(requestMessage, cancellationToken);
    }
}