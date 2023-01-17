using Infura.SDK.Common;
using Newtonsoft.Json;

namespace Infura.SDK.Organization
{
    /// <summary>
    /// 
    /// </summary>
    public class OrgNFTItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("attributes")]
        public Attribute[] Attributes { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("item_type_id")]
        public string ItemTypeId { get; set; }
    }
}