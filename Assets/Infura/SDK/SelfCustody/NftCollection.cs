using System;
using System.Threading.Tasks;
using Infura.SDK.Common;
using Infura.SDK.Organization;
using Infura.SDK.SelfCustody.Models;
using Newtonsoft.Json;

namespace Infura.SDK.SelfCustody
{
    public class NftCollection : IOrgLinkable
    {
        [JsonProperty("contract")]
        public string Contract { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("tokenType")]
        public TokenType TokenType { get; set; }

        [JsonIgnore]
        public CollectionData OrganizationCollectionData { get; private set; }
        
        public async Task TryLinkOrganization(OrgApiClient client)
        {
            var collections = await client.GetAllCollections();
            foreach (var collection in collections)
            {
                if (!String.Equals(collection.Contract.Address, Contract, StringComparison.CurrentCultureIgnoreCase)) continue;
                
                OrganizationCollectionData = collection;
                break;
            }
        }
    }
}