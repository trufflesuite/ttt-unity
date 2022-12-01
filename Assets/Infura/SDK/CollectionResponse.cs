using Newtonsoft.Json;

namespace Infura.SDK
{
    public class CollectionResponse
    {
        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }
        
        [JsonProperty("total")]
        public int Total { get; set; }
        
        [JsonProperty("chainName")]
        public string ChainName { get; set; }
        
        [JsonProperty("account")]
        public string Account { get; set; }
        
        [JsonProperty("collections")]
        public NftCollection[] Collections { get; set; }
    }
}