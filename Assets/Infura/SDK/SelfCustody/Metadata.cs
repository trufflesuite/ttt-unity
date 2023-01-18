using System;
using System.Collections.Generic;
using System.Linq;
using Infura.SDK.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Attribute = Infura.SDK.Common.Attribute;

namespace Infura.SDK.SelfCustody
{
    public class Metadata : IMetadata
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("attributes")]
        public Attribute[] Attributes { get; set; }
        
        [JsonProperty("image")]
        public string ImageUrl { get; set; }
        
        [JsonProperty("coverImage")]
        public string CoverImageUrl { get; set; }
        
        [JsonExtensionData]
#pragma warning disable CS0649
        private IDictionary<string, JToken> _extraStuff;
#pragma warning restore CS0649

        public Metadata(string name, string description, Attribute[] attributes = null)
        {
            if (attributes == null)
                attributes = Array.Empty<Attribute>();
            
            Name = name;
            Description = description;
            Attributes = attributes;
        }

        public void AddAttribute(Attribute attribute)
        {
            Attributes = Attributes.Append(attribute).ToArray();
        }
        
        public T GetExtraData<T>(string key)
        {
            if (_extraStuff == null || !_extraStuff.ContainsKey(key))
                return default;
            
            return _extraStuff[key].ToObject<T>();
        }

        public void SetExtraData<T>(string key, T value)
        {
            _extraStuff ??= new Dictionary<string, JToken>();

            if (_extraStuff.ContainsKey(key))
                _extraStuff.Remove(key);
            
            _extraStuff.Add(key, JToken.FromObject(value));
        }
    }
}