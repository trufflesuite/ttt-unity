using System;
using System.Threading.Tasks;
using Infura.SDK.Common;
using Infura.SDK.Models;
using Infura.SDK.Organization;
using Newtonsoft.Json;

namespace Infura.SDK
{
    /// <summary>
    /// A class that represents the collection metadata for an NFT collection.
    /// </summary>
    public class NftCollection : IOrgLinkable
    {
        /// <summary>
        /// The contract address for this NFT collection
        /// </summary>
        [JsonProperty("contract")]
        public string Contract { get; set; }
        
        /// <summary>
        /// The name of this NFT collection
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        
        /// <summary>
        /// The symbol of this NFT collection
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        /// <summary>
        /// The type of token this NFT collection contains
        /// </summary>
        [JsonProperty("tokenType")]
        public TokenType TokenType { get; set; }

        /// <summary>
        /// Additional organization information for this NFT collection. This is only
        /// populated if this collection belongs to an Organization that has been linked
        /// via the <see cref="ApiClient.LinkOrganization(string)"/>
        /// </summary>
        [JsonIgnore]
        public CollectionData OrganizationCollectionData { get; private set; }
        
        /// <summary>
        /// Attempts to find this NFT collection in the given Organization. If the collection is found
        /// in the organization, then the <see cref="OrganizationCollectionData"/> field is populated.
        /// </summary>
        /// <param name="client">The Organization API client to search in</param>
        public async Task<bool> TryLinkOrganization(OrgApiClient client)
        {
            var collections = await client.GetAllCollections();
            foreach (var collection in collections)
            {
                if (!string.Equals(collection.Contract.Address, Contract, StringComparison.CurrentCultureIgnoreCase)) continue;
                
                OrganizationCollectionData = collection;
                return true;
            }

            return false;
        }
    }
}