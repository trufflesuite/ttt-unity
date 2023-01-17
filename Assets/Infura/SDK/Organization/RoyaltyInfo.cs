using Newtonsoft.Json;

namespace Infura.SDK.Organization
{
    public class RoyaltyInfo
    {
        [JsonProperty("amount_bps")]
        public int AmountBps { get; set; }
        
        [JsonProperty("receiver")]
        public string Receiver { get; set; }
    }
}