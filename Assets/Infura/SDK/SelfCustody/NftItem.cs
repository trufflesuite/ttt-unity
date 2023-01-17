using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Infura.SDK.Common;
using Newtonsoft.Json;
using Attribute = Infura.SDK.Common.Attribute;

namespace Infura.SDK.SelfCustody
{
    public class NftItem
    {
        [JsonProperty("contract")]
        public string Contract { get; set; }
        
        [JsonProperty("tokenId")]
        public BigInteger TokenId { get; set; }
        
        [JsonProperty("supply")]
        public string Supply { get; set; }
        
        [JsonProperty("type")]
        public TokenType Type { get; set; }
        
        [JsonProperty("tokenHash")]
        public string TokenHash { get; set; }
        
        [JsonProperty("minterAddress")]
        public string MinterAddress { get; set; }
        
        [JsonProperty("blockNumberMinted")]
        public BigInteger? BlockNumberMinted { get; set; }
        
        [JsonProperty("transactionMinted")]
        public string TransactionMinted { get; set; }
        
        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
        
        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        public string ImageUrl
        {
            get
            {
                if (Metadata.ContainsKey("image"))
                    return Metadata["image"] as string;
                return "";
            }
        }

        public string CoverImageUrl
        {
            get
            {
                if (Metadata.ContainsKey("coverImage"))
                    return Metadata["coverImage"] as string;
                return "";
            }
        }

        public string Name
        {
            get
            {
                if (Metadata.ContainsKey("name"))
                    return Metadata["name"] as string;
                return "";
            }
        }

        public string Description
        {
            get
            {
                if (Metadata.ContainsKey("description"))
                    return Metadata["description"] as string;
                return "";
            }
        }

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