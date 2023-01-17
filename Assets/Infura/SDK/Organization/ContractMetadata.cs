using Newtonsoft.Json;

namespace Infura.SDK.Organization
{
    public class ContractMetadata
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("network_id")]
        public int NetworkId { get; set; }
    }
}