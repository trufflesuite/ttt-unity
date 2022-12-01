using System;
using System.Linq;
using Newtonsoft.Json;

namespace Infura.SDK
{
    public class SimpleMetadata : IMetadata
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("attributes")]
        public Attribute[] Attributes { get; set; }

        public SimpleMetadata(string name, string description, Attribute[] attributes = null)
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
    }
}