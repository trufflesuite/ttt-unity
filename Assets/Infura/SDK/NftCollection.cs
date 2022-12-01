using Newtonsoft.Json;

namespace Infura.SDK
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
        public string TokenType { get; set; }
    }
}