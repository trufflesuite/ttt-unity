using Newtonsoft.Json;

namespace Infura.SDK
{
    public class PendingTransactionState
    {
        [JsonProperty("on_chain_status")]
        public string OnChainStatus { get; set; }
        
        [JsonProperty("state")]
        public string State { get; set; }
        
        [JsonProperty("tx_hash")]
        public string TransactionHash { get; set; }
    }
}