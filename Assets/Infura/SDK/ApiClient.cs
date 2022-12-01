using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Infura.SDK
{
    public class ApiClient
    {
        public const string NFT_API_URL = "https://nft.api.infura.io";
        
        public Auth Auth { get; }
        
        public string ApiPath { get; }
        
        public IHttpService HttpClient { get; }
        
        public Ipfs IpfsClient { get; }

        public ApiClient(Auth auth)
        {
            this.Auth = auth ?? throw new ArgumentException("Expected non-null Auth object");

            this.ApiPath = $"/networks/{(int) auth.ChainId}";

            this.HttpClient = HttpServiceFactory.NewHttpService(NFT_API_URL, Auth.ApiAuth);

            IpfsClient = auth.Ipfs;
        }

        public async Task<NftAssetsResponse> GetNfts(string publicAddress)
        {
            if (string.IsNullOrWhiteSpace(publicAddress))
                throw new ArgumentException("Invalid account address");

            var apiUrl = $"{ApiPath}/accounts/{publicAddress}/assets/nfts";

            var json = await this.HttpClient.Get(apiUrl);

            var data = JsonConvert.DeserializeObject<NftAssetsResponse>(json);

            return data;
        }

        public async Task<NftAssetsResponse> GetNftsForCollection(string contractAddress)
        {
            if (string.IsNullOrWhiteSpace(contractAddress))
                throw new ArgumentException("Invalid contract address");

            var apiUrl = $"{ApiPath}/nfts/{contractAddress}/tokens";

            var json = await this.HttpClient.Get(apiUrl);

            var data = JsonConvert.DeserializeObject<NftAssetsResponse>(json);

            return data;
        }

        public async Task<GenericMetadataResponse<T>> GetTokenMetadata<T>(string contractAddress, string tokenId) where T : IMetadata
        {
            if (string.IsNullOrWhiteSpace(contractAddress))
                throw new ArgumentException("Invalid account address");

            var apiUrl = $"{ApiPath}/nfts/{contractAddress}/tokens/{tokenId}";
            var json = await HttpClient.Get(apiUrl);

            return JsonConvert.DeserializeObject<GenericMetadataResponse<T>>(json);
        }
        
        public async Task<GenericMetadataResponse<Metadata>> GetTokenMetadata(string contractAddress, string tokenId)
        {
            if (string.IsNullOrWhiteSpace(contractAddress))
                throw new ArgumentException("Invalid account address");

            var apiUrl = $"{ApiPath}/nfts/{contractAddress}/tokens/{tokenId}";
            var json = await HttpClient.Get(apiUrl);

            return JsonConvert.DeserializeObject<GenericMetadataResponse<Metadata>>(json);
        }

        public Task<string> StoreFile(string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException("File not found: " + file);

            if (IpfsClient == null)
                throw new Exception("IpfsClient not setup");

            return IpfsClient.UploadFile(file);
        }
        
        public Task<string> StoreMetadata(string metadata)
        {
            if (IpfsClient == null)
                throw new Exception("IpfsClient not setup");

            return IpfsClient.UploadContent(metadata);
        }
        
        public Task<string> StoreMetadata<T>(T metadata) where T : IMetadata
        {
            if (IpfsClient == null)
                throw new Exception("IpfsClient not setup");

            return StoreMetadata(JsonConvert.SerializeObject(metadata));
        }

        public Task<string> CreateFolder<T>(IEnumerable<T> metadata, bool isErc1155 = false) where T : IMetadata
        {
            if (IpfsClient == null)
                throw new Exception("IpfsClient not setup");

            return IpfsClient.UploadArray(metadata.Select(m => JsonConvert.SerializeObject(m)), isErc1155);
        }
    }
}