using System.Numerics;
using Infura.SDK.Common;
using Newtonsoft.Json;

namespace Infura.SDK.Models
{
    public class SearchNft : PaginatedResponse, IResponseSet<SearchNftResult>
    {
        /// <summary>
        /// The list of NFTs that match the search criteria on this page
        /// </summary>
        [JsonProperty("nfts")]
        public SearchNftResult[] Nfts { get; set; }

        /// <summary>
        /// The list of NFTs that match the search criteria on this page. Overloaded to implement IResponseSet.
        /// </summary>
        public SearchNftResult[] Data
        {
            get
            {
                return Nfts;
            }
        }
    }
}