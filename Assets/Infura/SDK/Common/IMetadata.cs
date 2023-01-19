using Infura.SDK.Common;
using Newtonsoft.Json;

namespace Infura.SDK.Common
{
    /// <summary>
    /// An interface representing standard Metadata for any NFT. Standard metadata is defined by the EIP-721 standard, which
    /// includes the following properties:
    /// * The name
    /// * The description
    /// * The image
    /// * The external URL
    /// * The attributes
    /// </summary>
    public interface IMetadata
    {
        /// <summary>
        /// The name of this NFT
        /// </summary>
        [JsonProperty("name")]
        string Name { get; set; }
        
        /// <summary>
        /// The description for this NFT
        /// </summary>
        [JsonProperty("description")]
        string Description { get; set; }
        
        /// <summary>
        /// The image url for this NFT
        /// </summary>
        [JsonProperty("image")]
        string ImageUrl { get; set; }

        /// <summary>
        /// The attributes for this NFT
        /// </summary>
        [JsonProperty("attributes")]
        Attribute[] Attributes { get; set; }
    }
}