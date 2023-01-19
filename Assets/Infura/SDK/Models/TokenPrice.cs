using System;
using System.Numerics;
using Newtonsoft.Json;

namespace Infura.SDK.Models
{
    /// <summary>
    /// A class that holds marketplace price data for a token.  
    /// </summary>
    public class TokenPrice
    {
        /// <summary>
        /// The transaction hash this trade was executed in.
        /// </summary>
        [JsonProperty("transactionHash")]
        public string TransactionHash { get; set; }
        
        /// <summary>
        /// The block timestamp this trade was executed.
        /// </summary>
        [JsonProperty("blockTimestamp")]
        public DateTime BlockTimestamp { get; set; }
        
        /// <summary>
        /// The block hash of the block this trade was executed in.
        /// </summary>
        [JsonProperty("blockHash")]
        public string BlockHash { get; set; }
        
        /// <summary>
        /// The block number of the block this trade was executed in.
        /// </summary>
        [JsonProperty("blockNumber")]
        public BigInteger BlockNumber { get; set; }
        
        /// <summary>
        /// The token ids traded.
        /// </summary>
        [JsonProperty("tokenIds")]
        public string[] TokenIds { get; set; }
        
        /// <summary>
        /// The seller of this trade.
        /// </summary>
        [JsonProperty("sellerAddress")]
        public string SellerAddress { get; set; }
        
        /// <summary>
        /// The buyer of this trade.
        /// </summary>
        [JsonProperty("buyerAddress")]
        public string BuyerAddress { get; set; }
        
        /// <summary>
        /// The contract address of the NFT Collection this trade was using.
        /// </summary>
        [JsonProperty("tokenAddress")]
        public string TokenAddress { get; set; }
        
        /// <summary>
        /// The contract address of the marketplace that was used to execute this trade.
        /// </summary>
        [JsonProperty("marketplaceAddress")]
        public string MarketplaceAddress { get; set; }
        
        /// <summary>
        /// The price of the trade.
        /// </summary>
        [JsonProperty("price")]
        public BigInteger Price { get; set; }
    }
}