using Infura.SDK.SelfCustody;
using Infura.SDK.SelfCustody.Models;
using Newtonsoft.Json;

namespace Infura.SDK.Organization
{
    public class CollectionResponse : ICursor, IResponseSet<NftCollection>
    {
        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }
        
        [JsonProperty("total")]
        public int Total { get; set; }
        
        [JsonProperty("chainName")]
        public string ChainName { get; set; }
        
        [JsonProperty("account")]
        public string Account { get; set; }
        
        [JsonProperty("collections")]
        public NftCollection[] Collections { get; set; }

        [JsonProperty("cursor")]
        public string Cursor { get; set; }

        public NftCollection[] Data
        {
            get
            {
                return Collections;
            }
        }
    }
}