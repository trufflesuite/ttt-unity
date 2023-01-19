using System.Numerics;
using Infura.SDK.Common;
using Newtonsoft.Json;

namespace Infura.SDK.Models
{
    /// <summary>
    /// A class representing a single Owner of a token
    /// </summary>
    public class OwnersResult
    {
        /// <summary>
        /// The token address of the collection this ownership is for
        /// </summary>
        [JsonProperty("tokenAddress")]
        public string TokenAddress { get; set; }
        
        /// <summary>
        /// The token Id this ownership is for
        /// </summary>
        [JsonProperty("tokenId")]
        public BigInteger TokenId { get; set; }
        
        /// <summary>
        /// The amount of this token owned by the owner
        /// </summary>
        [JsonProperty("amount")]
        public BigInteger Amount { get; set; }
        
        /// <summary>
        /// The address of the owner
        /// </summary>
        [JsonProperty("ownerOf")]
        public string Owner { get; set; }
        
        /// <summary>
        /// The hash of this token data
        /// </summary>
        [JsonProperty("tokenHash")]
        public string TokenHash { get; set; }
        
        /// <summary>
        /// The block number this token was minted
        /// </summary>
        [JsonProperty("blockNumberMinted")]
        public BigInteger BlockNumberMinted { get; set; }
        
        /// <summary>
        /// The block number this token was last transferred, resulting in this ownership
        /// </summary>
        [JsonProperty("blockNumber")]
        public BigInteger BlockNumber { get; set; }
        
        /// <summary>
        /// The type of token this is
        /// </summary>
        [JsonProperty("contractType")]
        public TokenType ContractType { get; set; }
        
        /// <summary>
        /// The name of this token
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
  
        /// <summary>
        /// The symbol for this token
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    
        /// <summary>
        /// The raw JSON string of the token metadata
        /// </summary>
        [JsonProperty("metadata")]
        public string MetadataJson { get; set; }
        
        /// <summary>
        /// The address of the minter of this token
        /// </summary>
        [JsonProperty("minterAddress")]
        public string MinterAddress { get; set; }
        
        /// <summary>
        /// Deserializes the metadata JSON into a Metadata object
        /// </summary>
        /// <typeparam name="T">The type of Metadata to deserialize the JSON as</typeparam>
        /// <returns>The deserialized Metadata</returns>
        public T MetadataAs<T>() where T : IMetadata
        {
            return JsonConvert.DeserializeObject<T>(MetadataJson);
        }
    }
}