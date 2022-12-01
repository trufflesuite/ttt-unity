using System.Collections.Generic;
using Newtonsoft.Json;

namespace Infura.SDK
{
    public class NftItem
    {
        [JsonProperty("contract")]
        public string Contract { get; set; }
        
        [JsonProperty("tokenId")]
        public string TokenId { get; set; }
        
        [JsonProperty("supply")]
        public string Supply { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}