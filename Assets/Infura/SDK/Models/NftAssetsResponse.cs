using System.Numerics;
using Infura.SDK.Common;
using Newtonsoft.Json;

namespace Infura.SDK.Models
{
    /// <summary>
    /// A paginated response for a list of NFT items. 
    /// </summary>
    public class NftAssetsResponse : PaginatedResponse, IResponseSet<NftItem>
    {
        /// <summary>
        /// The token type this response contains
        /// </summary>
        [JsonProperty("type")]
        public TokenType Type { get; set; }

        /// <summary>
        /// The list of NFT items in this response
        /// </summary>
        [JsonProperty("assets")]
        public NftItem[] Assets { get; set; }

        /// <summary>
        /// The list of NFT items in this response. Overloaded to implement IResponseSet
        /// </summary>
        public NftItem[] Data
        {
            get
            {
                return Assets;
            }
        }
    }
}