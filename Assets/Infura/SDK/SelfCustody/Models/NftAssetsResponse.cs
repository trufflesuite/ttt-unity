using System.Numerics;
using Infura.SDK.Common;
using Newtonsoft.Json;

namespace Infura.SDK.SelfCustody
{
    public class NftAssetsResponse
    {
        [JsonProperty("pageNumber")]
        public BigInteger PageNumber { get; set; }
        
        [JsonProperty("total")]
        public BigInteger Total { get; set; }
        
        [JsonProperty("network")]
        public Chains Network { get; set; }
        
        [JsonProperty("account")]
        public string Account { get; set; }
        
        [JsonProperty("type")]
        public TokenType Type { get; set; }
        
        [JsonProperty("cursor")]
        public string Cursor { get; set; }
        
        [JsonProperty("assets")]
        public NftItem[] Assets { get; set; }
    }
}