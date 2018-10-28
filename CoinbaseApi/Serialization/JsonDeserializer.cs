using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseApi.Serialization
{
    public class JsonNetDeseralizer : IDeserializer
    {
        private readonly JsonSerializerSettings settings;

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }

        public JsonNetDeseralizer(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content, settings);
        }
    }
}
