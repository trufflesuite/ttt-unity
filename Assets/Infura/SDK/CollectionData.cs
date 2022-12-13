using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Infura.SDK
{
    /// <summary>
    /// 
    /// </summary>
    public class CollectionData
    {
        /// <summary>
        /// 
        /// </summary>
        public const string RandomTokenAssignment = "RANDOM_AFTER_MINT";
        /// <summary>
        /// 
        /// </summary>
        public const string IncrementalTokenAssignment = "INCREMENTAL_AT_PROVISION";
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("can_create_items")]
        public bool CanCreateItems { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("default_item_type_id")]
        public string DefaultItemTypeId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("locked")]
        public bool Locked { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("organization_id")]
        public string OrganizationId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("royalty_info")]
        public RoyaltyInfo Royalty { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("tags")]
        public string[] Tags { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("token_assignment_strategy")]
        public string TokenAssignmentStrategy { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("token_contract")]
        public TokenContract Contract { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public CNFTClient Client { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<GenericMetadataResponse<T>> MetadataForToken<T>(string tokenId) where T : IMetadata
        {
            return Client.GetTokenMetadata<T>(Id, tokenId);
        }
    }
}