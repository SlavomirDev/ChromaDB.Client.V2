using Newtonsoft.Json;

using System.Net.Http;
using System.Threading.Tasks;

namespace MirDev.ChromaDB.Client.V2.Http
{
    internal static class HttpContentExtensions
    {
        public static async Task<T> ReadFromJsonAsync<T>(this HttpContent httpContent, JsonSerializerSettings jsonSerializerSettings = null)
        {
            var stringValue = await httpContent.ReadAsStringAsync();
            if (string.IsNullOrEmpty(stringValue))
                return default;

            return JsonConvert.DeserializeObject<T>(stringValue, jsonSerializerSettings);
        }
    }
}