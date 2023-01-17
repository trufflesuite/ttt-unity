using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infura.SDK.Common;
using Infura.SDK.Network;
using Newtonsoft.Json;

namespace Infura.SDK.Organization
{
    /// <summary>
    /// 
    /// </summary>
    public class OrgApiClient
    {
        /// <summary>
        /// 
        /// </summary>
        public const string NFT_API_URL = "https://platform.consensys-nft.com";

        public const string ADMIN_API_URL = "https://admin-api.consensys-nft.com";

        /// <summary>
        /// 
        /// </summary>
        public IHttpService HttpClient { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public IHttpService AdminHttpClient { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public Network.Ipfs IpfsClient { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="ipfs"></param>
        /// <exception cref="ArgumentException"></exception>
        public OrgApiClient(string apiKey, Network.Ipfs ipfs = null)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("Expected non-null apiKey string");

            this.HttpClient = HttpServiceFactory.NewHttpService(NFT_API_URL, apiKey, "CNFT-Api-Key");
            this.AdminHttpClient = HttpServiceFactory.NewHttpService(ADMIN_API_URL, apiKey, "CNFT-Api-Key");

            IpfsClient = ipfs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<CollectionData[]> GetAllCollections()
        {
            var apiUrl = $"/api/v2/collections";

            var json = await this.HttpClient.Get(apiUrl);

            var data = JsonConvert.DeserializeObject<CollectionData[]>(json);

            if (data == null) throw new Exception("Failed to get all collections");
            foreach (var productResponse in data)
            {
                productResponse.Client = this;
            }

            return data;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<CollectionData> GetCollection(string collectionId)
        {
            if (string.IsNullOrWhiteSpace(collectionId))
                throw new ArgumentException("Invalid collectionId");

            var apiUrl = $"/api/v2/collections/{collectionId}";

            var json = await this.HttpClient.Get(apiUrl);

            var data = JsonConvert.DeserializeObject<CollectionData>(json);

            if (data == null) throw new Exception("Could not find collection");
            data.Client = this;

            return data;
        }

        public async Task<ItemData[]> GetItemsFromCollection(string collectionId)
        {
            if (string.IsNullOrWhiteSpace(collectionId))
                throw new ArgumentException("Invalid collectionId");
            
            var apiUrl = $"/api/v2/collections/{collectionId}/items";

            var json = await this.HttpClient.Get(apiUrl);

            var data = JsonConvert.DeserializeObject<ItemData[]>(json);
            if (data == null) throw new Exception("Could not find collection");

            var items = new List<ItemData>();
            foreach (var itemData in data)
            {
                var id = itemData.Id;

                var itemApiUrl = $"/v1/items/{id}";

                var itemJson = await this.AdminHttpClient.Get(itemApiUrl);

                var adminItem = JsonConvert.DeserializeObject<ItemData>(itemJson);

                if (adminItem != null)
                {
                    adminItem.Client = this;

                    items.Add(adminItem);
                }
            }

            return items.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectionId"></param>
        /// <param name="tokenId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<GenericMetadataResponse<T>> GetTokenMetadata<T>(string collectionId, string tokenId) where T : IMetadata
        {
            if (string.IsNullOrWhiteSpace(collectionId))
                throw new ArgumentException("Invalid collectionId");
            
            if (string.IsNullOrWhiteSpace(tokenId))
                throw new ArgumentException("Invalid tokenId");

            var apiUrl = $"/api/v2/collections/{collectionId}/metadata/{tokenId}";

            var json = await this.HttpClient.Get(apiUrl);

            var data = JsonConvert.DeserializeObject<GenericMetadataResponse<T>>(json);

            if (data == null) throw new Exception("Could not find collection");

            return data;
        }
    }
}