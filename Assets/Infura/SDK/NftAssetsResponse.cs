using Newtonsoft.Json;

namespace Infura.SDK
{
    public class NftAssetsResponse
    {
        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }
        
        [JsonProperty("total")]
        public int Total { get; set; }
        
        [JsonProperty("network")]
        public string Network { get; set; }
        
        [JsonProperty("account")]
        public string Account { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("assets")]
        public NftItem[] Assets { get; set; }
    }
}