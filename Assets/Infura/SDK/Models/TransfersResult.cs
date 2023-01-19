using System;
using System.Numerics;
using Infura.SDK.Common;
using Newtonsoft.Json;

namespace Infura.SDK.Models
{
    /// <summary>
    /// A class representing a single transfer event on-chain
    /// </summary>
    public class TransfersResult
    {
        /// <summary>
        /// The address of the token contract where the transfer occured
        /// </summary>
        [JsonProperty("tokenAddress")]
        public string TokenAddress { get; set; }
        
        /// <summary>
        /// The token Id of the token that was transferred
        /// </summary>
        [JsonProperty("tokenId")]
        public BigInteger TokenId { get; set; }
        
        /// <summary>
        /// The address of the account that sent the token
        /// </summary>
        [JsonProperty("fromAddress")]
        public string FromAddress { get; set; }
        
        /// <summary>
        /// The address of the account that received the token
        /// </summary>
        [JsonProperty("toAddress")]
        public string ToAddress { get; set; }
        
        /// <summary>
        /// The type of token that was transferred
        /// </summary>
        [JsonProperty("contractType")]
        public TokenType ContractType { get; set; }
        
        /// <summary>
        /// The price the token was sold for
        /// </summary>
        [JsonProperty("price")]
        public BigInteger Price { get; set; }
        
        /// <summary>
        /// The amount of tokens that were transferred
        /// </summary>
        [JsonProperty("quantity")]
        public BigInteger Quantity { get; set; }
        
        /// <summary>
        /// The block number where the transfer occured
        /// </summary>
        [JsonProperty("blockNumber")]
        public BigInteger BlockNumber { get; set; }
        
        /// <summary>
        /// The block timestamp of when the transfer occured
        /// </summary>
        [JsonProperty("blockTimestamp")]
        public DateTime BlockTimestamp { get; set; }
        
        /// <summary>
        /// The block hash of the block where the transfer occured
        /// </summary>
        [JsonProperty("blockHash")]
        public string BlockHash { get; set; }
        
        /// <summary>
        /// The transaction hash of the transaction that caused the transfer
        /// </summary>
        [JsonProperty("transactionHash")]
        public string TransactionHash { get; set; }
        
        /// <summary>
        /// The type of transaction that caused the transfer
        /// </summary>
        [JsonProperty("transactionType")]
        public string TransactionType { get; set; }
    }
}