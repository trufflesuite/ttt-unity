using Newtonsoft.Json;

namespace Infura.SDK.Organization
{
    public class MintRequestResponse
    {
        public class MintRequestData
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            
            [JsonProperty("quantity")]
            public string Quantity { get; set; }
            
            [JsonProperty("transaction_id")]
            public string TransactionId { get; set; }
        }
        
        [JsonProperty("mint_requests")]
        public MintRequestData[] MintRequests { get; set; }
    }
}