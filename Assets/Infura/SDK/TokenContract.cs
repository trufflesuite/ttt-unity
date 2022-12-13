using Newtonsoft.Json;

namespace Infura.SDK
{
    public class TokenContract
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        
        [JsonProperty("capabilities")]
        public string[] Capabilities { get; set; }
        
        [JsonProperty("deploy_tx_id")]
        public string DeployTxId { get; set; }
        
        [JsonProperty("network_id")]
        public int NetworkId { get; set; }
        
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        
        [JsonProperty("tx_hash")]
        public string TxHash { get; set; }
    }
}