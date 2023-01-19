using System;
using System.Collections.Generic;
using System.Numerics;
using Infura.SDK.Common;
using Newtonsoft.Json;
using Attribute = Infura.SDK.Common.Attribute;

namespace Infura.SDK
{
    /// <summary>
    /// A class that represents a single NFT item
    /// </summary>
    public class NftItem
    {
        /// <summary>
        /// The contract address of the collection this NFT belongs to
        /// </summary>
        [JsonProperty("contract")]
        public string Contract { get; set; }
        
        /// <summary>
        /// The token ID of this NFT
        /// </summary>
        [JsonProperty("tokenId")]
        public BigInteger TokenId { get; set; }
        
        /// <summary>
        /// The max supply of this NFT
        /// </summary>
        [JsonProperty("supply")]
        public string Supply { get; set; }
        
        /// <summary>
        /// The type of NFT this is
        /// </summary>
        [JsonProperty("type")]
        public TokenType Type { get; set; }
        
        /// <summary>
        /// The hash of this NFT. This is only populated if this NFT
        /// instance came from a search result
        /// </summary>
        [JsonProperty("tokenHash")]
        public string TokenHash { get; set; }
        
        /// <summary>
        /// The address of the minter of this NFT. This is only populated if this NFT
        /// instance came from a search result
        /// </summary>
        [JsonProperty("minterAddress")]
        public string MinterAddress { get; set; }
        
        /// <summary>
        /// The block number this NFT was minted in. This is only populated if this NFT
        /// instance came from a search result
        /// </summary>
        [JsonProperty("blockNumberMinted")]
        public BigInteger? BlockNumberMinted { get; set; }
        
        /// <summary>
        /// The transaction hash this NFT was minted in. This is only populated if this NFT
        /// instance came from a search result
        /// </summary>
        [JsonProperty("transactionMinted")]
        public string TransactionMinted { get; set; }
        
        /// <summary>
        /// The DateTime this NFT was minted. This is only populated if this NFT
        /// instance came from a search result
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
        
        /// <summary>
        /// The Metadata this NFT has. 
        /// </summary>
        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        /// <summary>
        /// The image URL for this NFT, if one exists in the Metadata
        /// </summary>
        public string ImageUrl
        {
            get
            {
                if (Metadata.ContainsKey("image"))
                    return Metadata["image"] as string;
                return "";
            }
        }

        /// <summary>
        /// The cover image URL for this NFT, if one exists in the Metadata
        /// </summary>
        public string CoverImageUrl
        {
            get
            {
                if (Metadata.ContainsKey("coverImage"))
                    return Metadata["coverImage"] as string;
                return "";
            }
        }

        /// <summary>
        /// The name of this NFT, if one exists in the Metadata
        /// </summary>
        public string Name
        {
            get
            {
                if (Metadata.ContainsKey("name"))
                    return Metadata["name"] as string;
                return "";
            }
        }

        /// <summary>
        /// The description of this NFT, if one exists in the Metadata
        /// </summary>
        public string Description
        {
            get
            {
                if (Metadata.ContainsKey("description"))
                    return Metadata["description"] as string;
                return "";
            }
        }

        /// <summary>
        /// The array of attributes for this NFT, if one exists in the Metadata
        /// </summary>
        public Attribute[] Attributes
        {
            get
            {
                if (Metadata.ContainsKey("attributes"))
                    return Metadata["attributes"] as Attribute[];
                return Array.Empty<Attribute>();
            }
        }
    }
}