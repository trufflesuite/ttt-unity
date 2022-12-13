using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Infura.SDK
{
    /// <summary>
    /// 
    /// </summary>
    public class CNFTClient
    {
        /// <summary>
        /// 
        /// </summary>
        public const string NFT_API_URL = "https://platform.consensys-nft.com";

        /// <summary>
        /// 
        /// </summary>
        public string ApiPath { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public IHttpService HttpClient { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public Ipfs IpfsClient { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="ipfs"></param>
        /// <exception cref="ArgumentException"></exception>
        public CNFTClient(string apiKey, Ipfs ipfs = null)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("Expected non-null apiKey string");

            ApiPath = $"{NFT_API_URL}/api/v2";

            this.HttpClient = HttpServiceFactory.NewHttpService(NFT_API_URL, apiKey, "CNFT-Api-Key");

            IpfsClient = ipfs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<CollectionData[]> GetAllCollections()
        {
            var apiUrl = $"{ApiPath}/collections";

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

            var apiUrl = $"{ApiPath}/collections/{collectionId}";

            var json = await this.HttpClient.Get(apiUrl);

            var data = JsonConvert.DeserializeObject<CollectionData>(json);

            if (data == null) throw new Exception("Could not find collection");
            data.Client = this;

            return data;
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

            var apiUrl = $"{ApiPath}/collections/{collectionId}/metadata/{tokenId}";

            var json = await this.HttpClient.Get(apiUrl);

            var data = JsonConvert.DeserializeObject<GenericMetadataResponse<T>>(json);

            if (data == null) throw new Exception("Could not find collection");

            return data;
        }
    }
}