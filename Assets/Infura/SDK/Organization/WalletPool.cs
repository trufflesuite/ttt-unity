using Newtonsoft.Json;

namespace Infura.SDK.Organization
{
    public class WalletPool
    {
        [JsonProperty("contract")]
        public ContractMetadata Contract { get; set; }
        
        [JsonProperty("contract_pending_tx_id")]
        public string ContractPendingTxId { get; set; }
        
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("network_id")]
        public int NetworkId { get; set; }
        
        [JsonProperty("organization_id")]
        public string OrganizationId { get; set; }
        
        [JsonProperty("pool_size")]
        public int PoolSize { get; set; }
        
        [JsonProperty("wallets")]
        public WalletData[] Wallets { get; set; }
    }
}