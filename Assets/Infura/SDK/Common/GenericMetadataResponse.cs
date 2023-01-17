using Newtonsoft.Json;

namespace Infura.SDK.Common
{
    public class GenericMetadataResponse<T> where T : IMetadata
    {
        [JsonProperty("contract")]
        public string Contract { get; set; }
        
        [JsonProperty("tokenId")]
        public string TokenId { get; set; }
        
        [JsonProperty("metadata")]
        public T Metadata { get; set; }
    }
}