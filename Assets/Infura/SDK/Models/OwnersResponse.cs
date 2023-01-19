using Newtonsoft.Json;

namespace Infura.SDK.Models
{
    /// <summary>
    /// A paginated response for a list of owners
    /// </summary>
    public class OwnersResponse : PaginatedResponse, IResponseSet<OwnersResult>
    {
        /// <summary>
        /// The list of owners in this response
        /// </summary>
        [JsonProperty("owners")]
        public OwnersResult[] Owners { get; set; }

        /// <summary>
        /// The list of owners in this response. Overloaded to implement IResponseSet.
        /// </summary>
        public OwnersResult[] Data
        {
            get
            {
                return Owners;
            }
        }
    }
}