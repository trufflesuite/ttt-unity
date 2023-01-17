using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Infura.SDK.Common;
using Infura.SDK.Network;
using Infura.SDK.SelfCustody.Models;
using Newtonsoft.Json;

namespace Infura.SDK.SelfCustody
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
        public Network.Ipfs IpfsClient { get; }

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
        public IObservable<NftItem> GetNfts(string publicAddress)
        {
            if (string.IsNullOrWhiteSpace(publicAddress))
                throw new ArgumentException("Invalid account address");

            return Observable.Create<NftItem>(async (observer, cancel) =>
            {
                NftAssetsResponse data = null;
                do
                {
                    var apiUrl = $"{ApiPath}/accounts/{publicAddress}/assets/nfts{(data != null ? $"?cursor={data.Cursor}" : "")}";

                    var json = await this.HttpClient.Get(apiUrl);

                    data = JsonConvert.DeserializeObject<NftAssetsResponse>(json);
                    
                    if (data == null) continue;
                    
                    foreach (var item in data.Assets)
                    {
                        observer.OnNext(item);
                    }
                } while (data != null && !string.IsNullOrWhiteSpace(data.Cursor));
            });
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
        
        public IObservable<NftItem> SearchNfts(string query)
        {
            return Observable.Create<NftItem>(async (observer, cancel) =>
            {
                SearchNft data = null;
                do
                {
                    var apiUrl = $"{ApiPath}/nfts/search?query={query}{(data != null ? $"&cursor={data.Cursor}" : "")}";
                    var json = await HttpClient.Get(apiUrl);

                    data = JsonConvert.DeserializeObject<SearchNft>(json);

                    if (data == null) continue;

                    data.SearchQuery = query;
                    data.Client = this;
                    
                    foreach (var item in data.Nfts)
                    {
                        observer.OnNext(new NftItem()
                        {
                            BlockNumberMinted = item.BlockNumberMinted,
                            Contract = item.TokenAddress,
                            CreatedAt = item.CreatedAt,
                            Metadata = JsonConvert.DeserializeObject<Dictionary<string, object>>(item.MetadataJson),
                            MinterAddress = item.MinterAddress,
                            TokenHash = item.TokenHash,
                            TokenId = item.TokenId,
                            TransactionMinted = item.TransactionMinted,
                            Type = item.ContractType
                        });
                    }
                } while (data != null && !string.IsNullOrWhiteSpace(data.Cursor));
            });
        }
    }
}