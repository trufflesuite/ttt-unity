using Newtonsoft.Json;

namespace Infura.SDK.Models
{
    /// <summary>
    /// A paginated response for a list of NFT collections
    /// </summary>
    public class NftCollectionResponse : PaginatedResponse, IResponseSet<NftCollection>
    {
        /// <summary>
        /// The list of NFT collections in this response
        /// </summary>
        [JsonProperty("collections")]
        public NftCollection[] Collections { get; set; }

        /// <summary>
        /// The list of NFT collections in this response. Overloaded to implement IResponseSet
        /// </summary>
        public NftCollection[] Data
        {
            get
            {
                return Collections;
            }
        }
    }
}