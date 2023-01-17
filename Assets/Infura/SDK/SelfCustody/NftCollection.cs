using Infura.SDK.Common;
using Newtonsoft.Json;

namespace Infura.SDK.SelfCustody
{
    public class NftCollection
    {
        [JsonProperty("contract")]
        public string Contract { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("tokenType")]
        public TokenType TokenType { get; set; }
    }
}