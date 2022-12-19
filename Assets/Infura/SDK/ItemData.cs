using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Infura.SDK
{
    public class ItemData
    {
        public class MintRequest
        {
            [JsonProperty("item_id")]
            public string ItemId { get; set; }
            
            [JsonProperty("to_address")]
            public string ToAddress { get; set; }
            
            [JsonProperty("quantity")]
            public string Quantity { get; set; }
        }
        
        [JsonProperty("attributes")]
        public Dictionary<string, string> Attributes { get; set; }
        
        [JsonProperty("collection_id")]
        public string CollectionId { get; set; }
        
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("locked")]
        public bool Locked { get; set; }
        
        [JsonProperty("media")]
        public ItemDataMedia Media { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public CNFTClient Client { get; internal set; }

        public Task Lock()
        {
            try
            {
                var apiUrl = $"/v1/items/{Id}/lock";

                return Client.AdminHttpClient.Post(apiUrl, "{}");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("409") || e.Message.ToLower().Contains("conflict"))
                    return Task.CompletedTask;

                throw;
            }
        }
        
        public async Task<string> Mint(string owner, int quantity = 1)
        {
            if (!Locked)
            {
                await Lock();
            }

            var request = new MintRequest()
            {
                ItemId = Id,
                ToAddress = owner,
                Quantity = quantity.ToString()
            };
            
            var apiUrl = $"/v1/mint-requests";

            var json = await Client.AdminHttpClient.Post(apiUrl, JsonConvert.SerializeObject(request));

            var mintRequest = JsonConvert.DeserializeObject<MintRequestResponse>(json);

            var txApiUrl = $"/api/v2/transactions/{mintRequest.MintRequests[0].TransactionId}";
            do
            {
                var pendingTxJson = await Client.HttpClient.Get(txApiUrl);

                var pendingTx = JsonConvert.DeserializeObject<PendingTransactionState>(pendingTxJson);

                if (pendingTx != null && pendingTx.State == "COMPLETED")
                {
                    return pendingTx.TransactionHash;
                }

                await Task.Delay(500);
            } while (true);
        }
    }
}