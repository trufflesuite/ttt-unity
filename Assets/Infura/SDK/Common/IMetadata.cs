using Infura.SDK.Common;
using Newtonsoft.Json;

namespace Infura.SDK.Common
{
    public interface IMetadata
    {
        [JsonProperty("name")]
        string Name { get; set; }
        
        [JsonProperty("description")]
        string Description { get; set; }
        
        [JsonProperty("attributes")]
        Attribute[] Attributes { get; set; }
    }
}