using Newtonsoft.Json;

using System.Net.Http;
using System.Text;

namespace ChromaDB.Client.V2.Http
{
    internal sealed class JsonContent : StringContent
    {
        internal const string MediaType = "application/json";
        public JsonContent(object payload, JsonSerializerSettings serializerSettings = null) : base(JsonConvert.SerializeObject(payload, serializerSettings), Encoding.UTF8, MediaType) { }
        public JsonContent(object payload, Encoding encoding, JsonSerializerSettings serializerSettings = null) : base(JsonConvert.SerializeObject(payload, serializerSettings), encoding, MediaType) { }
    }
}