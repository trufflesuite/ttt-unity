using Newtonsoft.Json;

namespace Infura.SDK.Common
{
    /// <summary>
    /// Represents a generic JSON RPC Response that contains Metadata for a given token / contract. The metadata
    /// is automatically deserialized into the Metadata property as type T. 
    /// </summary>
    /// <typeparam name="T">The type of Metadata in this JSON RPC Response</typeparam>
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