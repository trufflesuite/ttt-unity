using System;
using System.Numerics;
using Infura.SDK.Common;
using Newtonsoft.Json;

namespace Infura.SDK.Models
{
    /// <summary>
    /// A single search result from the Infura API
    /// </summary>
    public class SearchNftResult
    {
        /// <summary>
        /// The token id of this result
        /// </summary>
        [JsonProperty("tokenId")]
        public BigInteger TokenId { get; set; }
        
        /// <summary>
        /// The token address for this result
        /// </summary>
        [JsonProperty("tokenAddress")]
        public string TokenAddress { get; set; }
        
        /// <summary>
        /// The raw JSON string of the token metadata
        /// </summary>
        [JsonProperty("metadata")]
        public string MetadataJson { get; set; }

        /// <summary>
        /// The token type of this result
        /// </summary>
        [JsonProperty("contractType")]
        public TokenType ContractType { get; set; }
        
        /// <summary>
        /// The hash of this result
        /// </summary>
        [JsonProperty("tokenHash")]
        public string TokenHash { get; set; }
        
        /// <summary>
        /// The address that minted this NFT
        /// </summary>
        [JsonProperty("minterAddress")]
        public string MinterAddress { get; set; }
        
        /// <summary>
        /// The block number where this NFT was minted
        /// </summary>
        [JsonProperty("blockNumberMinted")]
        public BigInteger? BlockNumberMinted { get; set; }
        
        /// <summary>
        /// The transaction hash where this NFT was minted
        /// </summary>
        [JsonProperty("transactionMinted")]
        public string TransactionMinted { get; set; }
        
        /// <summary>
        /// The DateTime this NFT was minted
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
    }
}