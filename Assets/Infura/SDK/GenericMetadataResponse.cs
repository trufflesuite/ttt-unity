using Newtonsoft.Json;

namespace Infura.SDK
{
    public class GenericMetadataResponse<T> where T : IMetadata
    {
        [JsonProperty("contract")]
        public string Contract { get; set; }
        
        [JsonProperty("tokenId")]
        public int TokenId { get; set; }
        
        [JsonProperty("metadata")]
        public T Metadata { get; set; }
    }
}