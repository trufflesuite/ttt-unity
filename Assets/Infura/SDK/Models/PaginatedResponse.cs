using System.Numerics;
using Infura.SDK.Common;
using Newtonsoft.Json;

namespace Infura.SDK.Models
{
    /// <summary>
    /// An abstract class representing a response that is paginated.
    /// </summary>
    public abstract class PaginatedResponse : ICursor
    {
        /// <summary>
        /// The current page number of this response
        /// </summary>
        [JsonProperty("pageNumber")]
        public BigInteger PageNumber { get; set; }
        
        /// <summary>
        /// The number of items on this page
        /// </summary>
        [JsonProperty("pageSize")]
        public BigInteger PageSize { get; set; }
        
        /// <summary>
        /// The total number of items in the original query result
        /// </summary>
        [JsonProperty("total")]
        public BigInteger Total { get; set; }
        
        /// <summary>
        /// The query cursor to use for the next page of results
        /// </summary>
        [JsonProperty("cursor")]
        public string Cursor { get; set; }
        
        /// <summary>
        /// The network this response is for
        /// </summary>
        [JsonProperty("network")]
        public Chains Network { get; set; }
        
        /// <summary>
        /// The account this response is for
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }
    }
}