using System;
using System.Numerics;
using Infura.SDK.Common;
using Newtonsoft.Json;

namespace Infura.SDK.SelfCustody.Models
{
    public class SearchNftResult
    {
        [JsonProperty("tokenId")]
        public BigInteger TokenId { get; set; }
        
        [JsonProperty("tokenAddress")]
        public string TokenAddress { get; set; }
        
        [JsonProperty("metadata")]
        public string MetadataJson { get; set; }
        
        [JsonProperty("contractType")]
        public TokenType ContractType { get; set; }
        
        [JsonProperty("tokenHash")]
        public string TokenHash { get; set; }
        
        [JsonProperty("minterAddress")]
        public string MinterAddress { get; set; }
        
        [JsonProperty("blockNumberMinted")]
        public BigInteger? BlockNumberMinted { get; set; }
        
        [JsonProperty("transactionMinted")]
        public string TransactionMinted { get; set; }
        
        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
    }
}