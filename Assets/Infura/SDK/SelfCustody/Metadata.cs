using System;
using System.Linq;
using Infura.SDK.Common;
using Newtonsoft.Json;
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
    }
}