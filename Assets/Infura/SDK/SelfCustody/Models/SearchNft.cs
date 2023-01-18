using System.Numerics;
using System.Threading.Tasks;
using Infura.SDK.Common;
using Newtonsoft.Json;

namespace Infura.SDK.SelfCustody.Models
{
    public class SearchNft : ICursor, IResponseSet<SearchNftResult>
    {
        [JsonProperty("total")]
        public BigInteger Total { get; set; }
        
        [JsonProperty("pageNumber")]
        public BigInteger PageNumber { get; set; }
        
        [JsonProperty("pageSize")]
        public BigInteger PageSize { get; set; }
        
        [JsonProperty("network")]
        public Chains Network { get; set; }
        
        [JsonProperty("cursor")]
        public string Cursor { get; set; }
        
        [JsonProperty("nfts")]
        public SearchNftResult[] Nfts { get; set; }
        
        public string SearchQuery { get; internal set; }
        
        internal ApiClient Client { get; set; }

        public SearchNftResult[] Data
        {
            get
            {
                return Nfts;
            }
        }
    }
}