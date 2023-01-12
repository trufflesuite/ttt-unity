using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Infura.SDK
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiClient
    {
        /// <summary>
        /// 
        /// </summary>
        public const string NFT_API_URL = "https://nft.api.infura.io";
        
        /// <summary>
        /// 
        /// </summary>
        public Auth Auth { get; }
        
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
        /// <param name="auth"></param>
        /// <exception cref="ArgumentException"></exception>
        public ApiClient(Auth auth)
        {
            this.Auth = auth ?? throw new ArgumentException("Expected non-null Auth object");

            this.ApiPath = $"/networks/{(int) auth.ChainId}";

            this.HttpClient = HttpServiceFactory.NewHttpService(NFT_API_URL, Auth.ApiAuth);

            IpfsClient = auth.Ipfs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicAddress"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<NftAssetsResponse> GetNfts(string publicAddress)
        {
            if (string.IsNullOrWhiteSpace(publicAddress))
                throw new ArgumentException("Invalid account address");

            var apiUrl = $"{ApiPath}/accounts/{publicAddress}/assets/nfts";

            var json = await this.HttpClient.Get(apiUrl);

            var data = JsonConvert.DeserializeObject<NftAssetsResponse>(json);

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<NftAssetsResponse> GetNftsForCollection(string contractAddress)
        {
            if (string.IsNullOrWhiteSpace(contractAddress))
                throw new ArgumentException("Invalid contract address");

            var apiUrl = $"{ApiPath}/nfts/{contractAddress}/tokens";

            var json = await this.HttpClient.Get(apiUrl);

            var data = JsonConvert.DeserializeObject<NftAssetsResponse>(json);

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <param name="tokenId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<GenericMetadataResponse<T>> GetTokenMetadata<T>(string contractAddress, string tokenId) where T : IMetadata
        {
            if (string.IsNullOrWhiteSpace(contractAddress))
                throw new ArgumentException("Invalid account address");

            var apiUrl = $"{ApiPath}/nfts/{contractAddress}/tokens/{tokenId}";
            var json = await HttpClient.Get(apiUrl);

            return JsonConvert.DeserializeObject<GenericMetadataResponse<T>>(json);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<GenericMetadataResponse<Metadata>> GetTokenMetadata(string contractAddress, string tokenId)
        {
            if (string.IsNullOrWhiteSpace(contractAddress))
                throw new ArgumentException("Invalid account address");

            var apiUrl = $"{ApiPath}/nfts/{contractAddress}/tokens/{tokenId}";
            var json = await HttpClient.Get(apiUrl);

            return JsonConvert.DeserializeObject<GenericMetadataResponse<Metadata>>(json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        public Task<string> StoreFile(string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException("File not found: " + file);

            if (IpfsClient == null)
                throw new Exception("IpfsClient not setup");

            return IpfsClient.UploadFile(file);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<string> StoreMetadata(string metadata)
        {
            if (IpfsClient == null)
                throw new Exception("IpfsClient not setup");

            return IpfsClient.UploadContent(metadata);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<string> StoreMetadata<T>(T metadata) where T : IMetadata
        {
            if (IpfsClient == null)
                throw new Exception("IpfsClient not setup");

            return StoreMetadata(JsonConvert.SerializeObject(metadata));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="isErc1155"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<string> CreateFolder<T>(IEnumerable<T> metadata, bool isErc1155 = false) where T : IMetadata
        {
            if (IpfsClient == null)
                throw new Exception("IpfsClient not setup");

            return CreateFolder(metadata.Select(m => JsonConvert.SerializeObject(m)), isErc1155);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="isErc1155"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<string> CreateFolder(IEnumerable<string> metadata, bool isErc1155 = false)
        {
            if (IpfsClient == null)
                throw new Exception("IpfsClient not setup");

            return IpfsClient.UploadArray(metadata, isErc1155);
        }
    }
}