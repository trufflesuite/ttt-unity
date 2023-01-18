using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Infura.SDK.Common;
using Infura.SDK.Network;
using Infura.SDK.Organization;
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
        public string ApiPath { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public IHttpService HttpClient { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public Network.Ipfs IpfsClient { get; }

        private Dictionary<string, OrgApiClient> _orgApiClients = new Dictionary<string, OrgApiClient>();

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

        public void UpdateChain(Chains chains)
        {
            this.Auth.ChainId = chains;
            this.ApiPath = $"/networks/{(int) Auth.ChainId}";
        }

        public Task<List<NftItem>> GetNfts(string publicAddress) => GetNftsObservable(publicAddress).AsList();


        public IObservable<NftItem> GetNftsObservable(string publicAddress) =>
            ObservablePageante<NftAssetsResponse, NftItem>($"{ApiPath}/accounts/{publicAddress}/assets/nfts");

        public Task<List<NftItem>> GetNftsForCollection(string contractAddress) =>
            GetNftsForCollectionObservable(contractAddress).AsList();

        public IObservable<NftItem> GetNftsForCollectionObservable(string contractAddress) =>
            ObservablePageante<NftAssetsResponse, NftItem>($"{ApiPath}/nfts/{contractAddress}/tokens");

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

        public Task<NftCollection> GetCollectionForItem(NftItem item)
        {
            return GetCollection(item.Contract);
        }
        
        public async Task<NftCollection> GetCollection(string contractAddress)
        {
            if (string.IsNullOrWhiteSpace(contractAddress))
                throw new ArgumentException("Invalid account address");

            var apiUrl = $"{ApiPath}/nfts/{contractAddress}";
            var json = await HttpClient.Get(apiUrl);

            var collection = JsonConvert.DeserializeObject<NftCollection>(json);

            if (collection == null) return null;
            
            foreach (var org in _orgApiClients.Values)
                await collection.TryLinkOrganization(org);

            return collection;
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

        public Task<List<NftItem>> SearchNfts(string query) => SearchNftsObservable(query).AsList();

        public IObservable<NftItem> SearchNftsObservable(string query) =>
            ObservablePageante<SearchNft, SearchNftResult, NftItem>($"{ApiPath}/nfts/search?query={query}", item =>
                new NftItem()
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

        public OrgApiClient LinkOrganization(string organizationId)
        {
            if (_orgApiClients.ContainsKey(organizationId))
                return _orgApiClients[organizationId];
            
            var orgApi = new OrgApiClient(organizationId, IpfsClient);
            _orgApiClients.Add(organizationId, orgApi);
            return orgApi;
        }

        public IObservable<T> ObservablePageante<TR, T>(string apiUrl)
            where TR : ICursor, IResponseSet<T>
        {
            return ObservablePageante<TR, T, T>(apiUrl, arg => arg);
        }

        public IObservable<R> ObservablePageante<TR, T, R>(string apiUrl, Func<T, R> selector) where TR : ICursor, IResponseSet<T>
        {
            if (selector == null)
                throw new ArgumentException("Selector is null");

            return Observable.Create<R>(async (observer, cancel) =>
            {
                TR data = default;
                do
                {
                    var cursor = data != null ? $"{(apiUrl.Contains("?") ? "&" : "?")}cursor={data.Cursor}" : "";
                    var fullUrl = $"{apiUrl}{cursor}";
                    
                    var json = await HttpClient.Get(fullUrl);
                    
                    data = JsonConvert.DeserializeObject<TR>(json);

                    if (data == null) continue;

                    foreach (var item in data.Data)
                    {
                        var result = selector(item);
                        
                        if (result is IOrgLinkable linkable)
                        {
                            foreach (var org in _orgApiClients.Values)
                                await linkable.TryLinkOrganization(org);
                        }
                        
                        observer.OnNext(result);
                    }
                } while (!cancel.IsCancellationRequested && data != null && !string.IsNullOrWhiteSpace(data.Cursor));
            });
        }
    }
}