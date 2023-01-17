using Newtonsoft.Json;

namespace Infura.SDK.Organization
{
    public class WalletData
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        
        [JsonProperty("auto_fund")]
        public bool AutoFund { get; set; }
        
        [JsonProperty("custodial")]
        public bool Ccustodial { get; set; }
        
        [JsonProperty("funding_wallet_id")]
        public string FundingWalletId { get; set; }
        
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("network_ids")]
        public int[] NetworkIds { get; set; }
        
        [JsonProperty("restrict_networks")]
        public bool RestrictNetworks { get; set; }
    }
}