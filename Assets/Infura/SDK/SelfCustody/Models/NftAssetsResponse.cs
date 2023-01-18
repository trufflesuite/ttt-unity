﻿using System.Numerics;
using Infura.SDK.Common;
using Infura.SDK.SelfCustody.Models;
using Newtonsoft.Json;

namespace Infura.SDK.SelfCustody
{
    public class NftAssetsResponse : ICursor, IResponseSet<NftItem>
    {
        [JsonProperty("pageNumber")]
        public BigInteger PageNumber { get; set; }
        
        [JsonProperty("total")]
        public BigInteger Total { get; set; }
        
        [JsonProperty("network")]
        public Chains Network { get; set; }
        
        [JsonProperty("account")]
        public string Account { get; set; }
        
        [JsonProperty("type")]
        public TokenType Type { get; set; }
        
        [JsonProperty("cursor")]
        public string Cursor { get; set; }
        
        [JsonProperty("assets")]
        public NftItem[] Assets { get; set; }

        public NftItem[] Data
        {
            get
            {
                return Assets;
            }
        }
    }
}