using Newtonsoft.Json;

namespace Infura.SDK.Models
{
    /// <summary>
    /// A paginated response for a list of transfers
    /// </summary>
    public class TransfersResponse : PaginatedResponse, IResponseSet<TransfersResult>
    {
        /// <summary>
        /// The list of transfers in this response
        /// </summary>
        [JsonProperty("transfers")]
        public TransfersResult[] Transfers { get; set; }
        
        /// <summary>
        /// The list of transfers in this response. Overloaded to implement IResponseSet
        /// </summary>
        public TransfersResult[] Data
        {
            get
            {
                return Transfers;
            }
        }
    }
}