using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Infura.SDK.Common;
using Infura.SDK.Models;
using Infura.SDK.Network;
using Infura.SDK.Organization;
using Newtonsoft.Json;

namespace Infura.SDK
{
    /// <summary>
    /// The Infura NFT API client.
    /// </summary>
    public class ApiClient
    {
        #region Constants
        /// <summary>
        /// The base URL for the Infura NFT API
        /// </summary>
        private const string NftApiUrl = "https://nft.api.infura.io";
        #endregion
        
        #region Fields
        /// <summary>
        /// The Authentication configuration this API Client is using for requests
        /// </summary>
        public Auth Auth { get; }
        
        /// <summary>
        /// The base URL for the Infura NFT API this API Client is using
        /// </summary>
        public string ApiPath { get; private set; }
        
        /// <summary>
        /// The HTTP Client this API Client is using for requests
        /// </summary>
        public IHttpService HttpClient { get; }
        
        /// <summary>
        /// The IPFS Client this API Client is using for IPFS requests
        /// </summary>
        public Network.Ipfs IpfsClient { get; }
        #endregion

        #region Private Fields
        private Dictionary<string, OrgApiClient> _orgApiClients = new Dictionary<string, OrgApiClient>();
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new API Client with the given auth credentials and configuration
        /// </summary>
        /// <param name="auth">The authentication configuration/credentials to use</param>
        /// <exception cref="ArgumentException">If no auth object is given</exception>
        public ApiClient(Auth auth)
        {
            this.Auth = auth ?? throw new ArgumentException("Expected non-null Auth object");

            this.ApiPath = $"/networks/{(int) auth.ChainId}";

            this.HttpClient = HttpServiceFactory.NewHttpService(NftApiUrl, Auth.ApiAuth);

            IpfsClient = auth.Ipfs;
        }
        #endregion
        
        #region Transfers
        /// <summary>
        /// Get a list of all transfers from a block to a block. This task completes when the full list of transfers
        /// is available, so it may take a while if the block range yields a large number of transfers.  For a "lazy-loading"
        /// version of this function, use <see cref="GetTransfersObservable (BigInteger, BigInteger)"/>
        /// </summary>
        /// <param name="fromBlock">The start block number to get transfers from</param>
        /// <param name="toBlock">The end block number to get transfers from</param>
        /// <returns>A task that returns a full list of transfers between Blocks fromBlock - toBlock</returns>
        public Task<List<TransfersResult>> GetTransfers(BigInteger fromBlock, BigInteger toBlock) =>
            GetTransfersObservable(fromBlock, toBlock).AsListAsync();

        /// <summary>
        /// Get all transfers from a block to a block. This returns an
        /// observable that emits each transfer as it is obtained from the NFT API. This is useful for
        /// "lazy-loading" transfers, as the function will complete as soon as the Observable is created and started.
        /// For a "blocking" version of this function, use <see cref="GetTransfers (BigInteger, BigInteger)"/>
        /// </summary>
        /// <param name="fromBlock">The start block number to get transfers from</param>
        /// <param name="toBlock">The end block number to get transfers from</param>
        /// <returns>An observable that emits transfers from a block to a block. This observable completes when all transfers have been emitted.</returns>
        public IObservable<TransfersResult> GetTransfersObservable(BigInteger fromBlock, BigInteger toBlock) => 
            ObservablePaginate<TransfersResponse, TransfersResult>(
                $"{ApiPath}/nfts/transfers?fromBlock={fromBlock}&toBlock={toBlock}"
                );

        /// <summary>
        /// Get a list of all transfers from a single block by block number. This task completes when the full list of transfers
        /// is available, so it may take a while if the block yields a large number of transfers.  For a "lazy-loading"
        /// version of this function, use <see cref="GetTransfersByBlockObservable (BigInteger)"/>
        /// </summary>
        /// <param name="blockNumber">The block number to get transfers from</param>
        /// <returns>A task that returns a full list of transfers from the block at the specified blockNumber.</returns>
        public Task<List<TransfersResult>> GetTransfersByBlock(BigInteger blockNumber) =>
            GetTransfersByBlockObservable(blockNumber).AsListAsync();
        
        /// <summary>
        /// Get all transfers from a single block by block number. This returns an
        /// observable that emits each transfer as it is obtained from the NFT API. This is useful for
        /// "lazy-loading" transfers, as the function will complete as soon as the Observable is created and started.
        /// For a "blocking" version of this function, use <see cref="GetTransfersByBlock (BigInteger)"/>
        /// </summary>
        /// <param name="blockNumber">The block number to get transfers from</param>
        /// <returns>An observable that emits transfers from the block at the specified blockNumber.
        /// This observable completes when all transfers have been emitted.</returns>
        public IObservable<TransfersResult> GetTransfersByBlockObservable(BigInteger blockNumber) => 
            ObservablePaginate<TransfersResponse, TransfersResult>(
                $"{ApiPath}/nfts/block/transfers?blockHashNumber={blockNumber}"
            );

        /// <summary>
        /// Get all transfers from a user by their wallet address. This task completes when the full list of transfers
        /// is available, so it may take a while if the wallet address yields a large number of transfers. For a "lazy-loading"
        /// version of this function, use <see cref="GetTransfersByWalletObservable (string)"/>
        /// </summary>
        /// <param name="publicAddress">The wallet address to query transfers from</param>
        /// <returns>A task that returns a full list of transfers from the specified wallet address.</returns>
        public Task<List<TransfersResult>> GetTransfersByWallet(string publicAddress) =>
            GetTransfersByWalletObservable(publicAddress).AsListAsync();
        
        /// <summary>
        /// Get all transfers from a user by their wallet address. This returns an
        /// observable that emits each transfer as it is obtained from the NFT API. This is useful for
        /// "lazy-loading" transfers, as the function will complete as soon as the Observable is created and started.
        /// For a "blocking" version of this function, use <see cref="GetTransfersByBlock (BigInteger)"/>
        /// </summary>
        /// <param name="publicAddress">The wallet address to query transfers from</param>
        /// <returns>An observable that emits transfers from the specified wallet address.
        /// This observable completes when all transfers have been emitted.</returns>
        public IObservable<TransfersResult> GetTransfersByWalletObservable(string publicAddress) => 
            ObservablePaginate<TransfersResponse, TransfersResult>(
                $"{ApiPath}/accounts/{publicAddress}/assets/transfers"
            );

        /// <summary>
        /// Get all transfers for an NFT from a specific NFT collection. This task completes when the full list of transfers
        /// is available, so it may take a while if the NFT yields a large number of transfers. For a "lazy-loading"
        /// version of this function, use <see cref="GetTransfersForNftObservable(string,string)"/>
        /// </summary>
        /// <param name="contractAddress">The contract address of the NFT collection the token belogns to</param>
        /// <param name="tokenId">The token Id of the NFT to get transfers for</param>
        /// <returns>A task that returns a full list of transfers from the specified NFT.</returns>
        public Task<List<TransfersResult>> GetTransfersForNft(string contractAddress, string tokenId) =>
            GetTransfersForNftObservable(contractAddress, tokenId).AsListAsync();
        
        /// <summary>
        /// Get all transfers for an NFT from a specific NFT collection. This returns an
        /// observable that emits each transfer as it is obtained from the NFT API. This is useful for
        /// "lazy-loading" transfers, as the function will complete as soon as the Observable is created and started.
        /// For a "blocking" version of this function, use <see cref="GetTransfersByBlock (BigInteger)"/>
        /// </summary>
        /// <param name="contractAddress">The contract address of the NFT collection the token belogns to</param>
        /// <param name="tokenId">The token Id of the NFT to get transfers for</param>
        /// <returns>An observable that emits transfers from the specified NFT.
        /// This observable completes when all transfers have been emitted.</returns>
        public IObservable<TransfersResult> GetTransfersForNftObservable(string contractAddress, string tokenId) => 
            ObservablePaginate<TransfersResponse, TransfersResult>(
                $"{ApiPath}/nfts/{contractAddress}/tokens/{tokenId}/transfers"
            );
        
        /// <summary>
        /// Get all transfers for an NFT from a specific NFT collection. This task completes when the full list of transfers
        /// is available, so it may take a while if the NFT yields a large number of transfers. For a "lazy-loading"
        /// version of this function, use <see cref="GetTransfersForNftObservable(string,string)"/>
        /// </summary>
        /// <param name="nftItem">The NFT to get transfers for</param>
        /// <returns>A task that returns a full list of transfers from the specified NFT.</returns>
        public Task<List<TransfersResult>> GetTransfersForNft(NftItem nftItem) =>
            GetTransfersForNftObservable(nftItem).AsListAsync();

        /// <summary>
        /// Get all transfers for an NFT from a specific NFT collection. This returns an
        /// observable that emits each transfer as it is obtained from the NFT API. This is useful for
        /// "lazy-loading" transfers, as the function will complete as soon as the Observable is created and started.
        /// For a "blocking" version of this function, use <see cref="GetTransfersByBlock (BigInteger)"/>
        /// </summary>
        /// <param name="nftItem">The NFT to get transfers for</param>
        /// <returns>An observable that emits transfers from the specified NFT.
        /// This observable completes when all transfers have been emitted.</returns>
        public IObservable<TransfersResult> GetTransfersForNftObservable(NftItem nftItem) =>
            GetTransfersForNftObservable(nftItem.Contract, nftItem.TokenId.ToString());
        
        /// <summary>
        /// Get all transfers for all NFTs in a given NFT collection. This task completes when the full list of transfers
        /// is available, so it may take a while if the NFT collection yields a large number of transfers. For a "lazy-loading"
        /// version of this function, use <see cref="GetTransfersForNftCollectionObservable(string)"/>
        /// </summary>
        /// <param name="contractAddress">The contract address of the NFT collection to get transfers from</param>
        /// <returns>A task that returns the full list of transfers from the specified NFT collection.</returns>
        public Task<List<TransfersResult>> GetTransfersForNftCollection(string contractAddress) =>
            GetTransfersForNftCollectionObservable(contractAddress).AsListAsync();
        
        /// <summary>
        /// Get all transfers for all NFTs in a given NFT collection. This returns an
        /// observable that emits each transfer as it is obtained from the NFT API. This is useful for
        /// "lazy-loading" transfers, as the function will complete as soon as the Observable is created and started.
        /// For a "blocking" version of this function, use <see cref="GetTransfersByBlock (BigInteger)"/>
        /// </summary>
        /// <param name="contractAddress">The contract address of the NFT collection to get transfers from</param>
        /// <returns>An observable that emits transfers from the specified NFT collection.
        /// This observable completes when all transfers have been emitted.</returns>
        public IObservable<TransfersResult> GetTransfersForNftCollectionObservable(string contractAddress) => 
            ObservablePaginate<TransfersResponse, TransfersResult>(
                $"{ApiPath}/nfts/{contractAddress}/transfers"
            );

        /// <summary>
        /// Get all transfers for all NFTs in a given NFT collection. This task completes when the full list of transfers
        /// is available, so it may take a while if the NFT collection yields a large number of transfers. For a "lazy-loading"
        /// version of this function, use <see cref="GetTransfersForNftCollectionObservable(string)"/>
        /// </summary>
        /// <param name="collection">The NFT collection to get transfers from</param>
        /// <returns>A task that returns the full list of transfers from the specified NFT collection.</returns>
        public Task<List<TransfersResult>> GetTransfersForNftCollection(NftCollection collection) =>
            GetTransfersForNftCollectionObservable(collection).AsListAsync();

        /// <summary>
        /// Get all transfers for all NFTs in a given NFT collection. This returns an
        /// observable that emits each transfer as it is obtained from the NFT API. This is useful for
        /// "lazy-loading" transfers, as the function will complete as soon as the Observable is created and started.
        /// For a "blocking" version of this function, use <see cref="GetTransfersByBlock (BigInteger)"/>
        /// </summary>
        /// <param name="collection">The NFT collection to get transfers from</param>
        /// <returns>An observable that emits transfers from the specified NFT collection.
        /// This observable completes when all transfers have been emitted.</returns>
        public IObservable<TransfersResult> GetTransfersForNftCollectionObservable(NftCollection collection) =>
            GetTransfersForNftCollectionObservable(collection.Contract);

        /// <summary>
        /// Get all transfers for all NFTs in a given NFT collection. This task completes when the full list of transfers
        /// is available, so it may take a while if the NFT collection yields a large number of transfers. For a "lazy-loading"
        /// version of this function, use <see cref="GetTransfersForNftCollectionObservable(string)"/>
        /// </summary>
        /// <param name="nftItem">The NFT that belongs to the NFT collection to get transfers from</param>
        /// <returns>A task that returns the full list of transfers from the specified NFT collection.</returns>
        public Task<List<TransfersResult>> GetTransfersForNftCollection(NftItem nftItem) =>
            GetTransfersForNftCollectionObservable(nftItem).AsListAsync();
        
        /// <summary>
        /// Get all transfers for all NFTs in a given NFT collection. This returns an
        /// observable that emits each transfer as it is obtained from the NFT API. This is useful for
        /// "lazy-loading" transfers, as the function will complete as soon as the Observable is created and started.
        /// For a "blocking" version of this function, use <see cref="GetTransfersByBlock (BigInteger)"/>
        /// </summary>
        /// <param name="nftItem">The NFT that belongs to the NFT collection to get transfers from</param>
        /// <returns>An observable that emits transfers from the specified NFT collection.
        /// This observable completes when all transfers have been emitted.</returns>
        public IObservable<TransfersResult> GetTransfersForNftCollectionObservable(NftItem nftItem) =>
            GetTransfersForNftCollectionObservable(nftItem.Contract);
        #endregion
        
        #region Metadata
        /// <summary>
        /// Get a list of all NFTs a user owns at a given wallet address. This task completes when the full list is
        /// available, so it may take a while to complete if the user owns a large number of NFTs. For a "lazy-loading"
        /// version of this function, use <see cref="GetNftsObservable (string)"/>
        /// </summary>
        /// <param name="publicAddress">The wallet address to query NFTs for</param>
        /// <returns>A task that returns the full list of NFTs owned by the given wallet address</returns>
        public Task<List<NftItem>> GetNfts(string publicAddress) => GetNftsObservable(publicAddress).AsListAsync();


        /// <summary>
        /// Get all NFTs a user owns at a given wallet address. This returns an observable that emits each NFT as it is
        /// obtained from the NFT API. This is useful for "lazy-loading" NFTs, as the function will complete as soon as the
        /// Observable is created and started. For a "blocking" version of this function, use <see cref="GetNfts (string)"/>
        /// </summary>
        /// <param name="publicAddress">The wallet address to query NFTs for</param>
        /// <returns>An observable that emits NFTs the user owns. This observable completes when all NFTs have been
        /// emitted.</returns>
        public IObservable<NftItem> GetNftsObservable(string publicAddress) =>
            ObservablePaginate<NftAssetsResponse, NftItem>($"{ApiPath}/accounts/{publicAddress}/assets/nfts");
        
        /// <summary>
        /// Grab and deserialize specific metadata for a NFT at the given contract address and token ID. This function
        /// will deserialize the JSON into the given type T, where T is of type IMetadata.
        /// </summary>
        /// <param name="contractAddress">The contract address / collection the NFT belongs to</param>
        /// <param name="tokenId">The token ID of the NFT to get Metadata for</param>
        /// <typeparam name="T">The type of Metadata to deserialize and return.</typeparam>
        /// <returns>The Metadata for the NFT as type T</returns>
        /// <exception cref="ArgumentException">If the contract address is not provided or tokenId is not provided</exception>
        public async Task<T> GetTokenMetadata<T>(string contractAddress, string tokenId) where T : IMetadata
        {
            if (string.IsNullOrWhiteSpace(contractAddress) || string.IsNullOrWhiteSpace(tokenId))
                throw new ArgumentException($"Invalid address: contractAddress ({contractAddress}) or tokenId ({tokenId}) is null or empty");

            var apiUrl = $"{ApiPath}/nfts/{contractAddress}/tokens/{tokenId}";

            var data = await Get<GenericMetadataResponse<T>>(apiUrl);

            return data != null ? data.Metadata : default;
        }
        
        /// <summary>
        /// Grab and deserialize specific metadata for a given NFT item. This function
        /// will deserialize the JSON into the given type T, where T is of type IMetadata.
        /// </summary>
        /// <param name="nftItem">The NFT to grab Metadata for</param>
        /// <typeparam name="T">The type of Metadata to deserialize and return.</typeparam>
        /// <returns>The Metadata for the NFT as type T</returns>
        public Task<T> GetTokenMetadata<T>(NftItem nftItem) where T : IMetadata =>
            GetTokenMetadata<T>(nftItem.Contract, nftItem.TokenId.ToString());

        /// <summary>
        /// Grab and deserialize specific metadata for a given NFT item. This function
        /// will deserialize the JSON into the type Metadata. The Metadata class ia a general-purpose
        /// Metadata class that covers most usecases. If you need more specific Metadata, use the
        /// generic version of this function <see cref="GetTokenMetadata{T} (string, string)"/>
        /// </summary>
        /// <param name="contractAddress">The contract address / collection the NFT belongs to</param>
        /// <param name="tokenId">The token ID of the NFT to get Metadata for</param>
        /// <returns>The Metadata for the NFT</returns>
        /// <exception cref="ArgumentException">If the contract address is not provided or tokenId is not provided</exception>
        public Task<Metadata> GetTokenMetadata(string contractAddress, string tokenId) =>
            GetTokenMetadata<Metadata>(contractAddress, tokenId);

        /// <summary>
        /// Grab and deserialize specific metadata for a given NFT item. This function
        /// will deserialize the JSON into the type Metadata. The Metadata class ia a general-purpose
        /// Metadata class that covers most usecases. If you need more specific Metadata, use the
        /// generic version of this function <see cref="GetTokenMetadata{T} (NftItem)"/>
        /// </summary>
        /// <param name="nftItem">The NFT to grab Metadata for</param>
        /// <returns>The Metadata for the NFT</returns>
        public Task<Metadata> GetTokenMetadata(NftItem nftItem) =>
            GetTokenMetadata<Metadata>(nftItem.Contract, nftItem.TokenId.ToString());
        
        /// <summary>
        /// Search for NFTs using a given query string and get a list of all results. The query string can be a simple.
        /// For example, "CryptoKitty". This task completes when the full search result list is available, so it
        /// may take a while if the search query yields a large number of results.  For a "lazy-loading"
        /// version of this function, use <see cref="SearchNftsObservable (string)"/>
        /// </summary>
        /// <param name="query">The query string to search with</param>
        /// <returns>A task that returns the full list of results from the search query</returns>
        public Task<List<NftItem>> SearchNfts(string query) => SearchNftsObservable(query).AsListAsync();

        /// <summary>
        /// Search for NFTs using the given query string. The query string can be a simple. For example, "CryptoKitty". This will
        /// return an Observable where each result from the search will be emitted. The Observable will complete when
        /// all search results have been processed.
        /// </summary>
        /// <param name="query">The query string to search with</param>
        /// <returns>An observable that emits NFT results from the search query</returns>
        public IObservable<NftItem> SearchNftsObservable(string query) =>
            ObservablePaginate<SearchNft, SearchNftResult, NftItem>($"{ApiPath}/nfts/search?query={query}", item =>
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
        #endregion

        #region Collections
        /// <summary>
        /// Get a list of all NFT collections where the given user owns at least one NFT from each collection. This
        /// task completes when the full list is available, so it may take a while to complete if the user owns a large
        /// number of NFT collections. For a "lazy-loading" version of this function, use <see cref="GetNftCollectionsObservable (string)"/>
        /// </summary>
        /// <param name="publicAddress">The wallet address of the user to query collections for</param>
        /// <returns>A task that returns the full list of NFT collections where the user owns at least one NFT from each collection.</returns>
        public Task<List<NftCollection>> GetNftCollections(string publicAddress) => GetNftCollectionsObservable(publicAddress).AsListAsync();
        
        /// <summary>
        /// Get all NFT collections where the given user owns at least one NFT from each collection. This returns an
        /// observable that emits each NFT collection as it is obtained from the NFT API. This is useful for
        /// "lazy-loading" NFT collections, as the function will complete as soon as the Observable is created and started.
        /// For a "blocking" version of this function, use <see cref="GetNftCollections (string)"/>
        /// </summary>
        /// <param name="publicAddress">The wallet address of the user to query collections for</param>
        /// <returns>An observable that emits NFT collections where the user owns at least one NFT from each collection.
        /// This observable completes when all NFTs have been emitted.</returns>
        public IObservable<NftCollection> GetNftCollectionsObservable(string publicAddress) =>
            ObservablePaginate<NftCollectionResponse, NftCollection>($"{ApiPath}/accounts/{publicAddress}/assets/collections");

        /// <summary>
        /// Get a list of all NFTs that are from a specific collection / contract address. This task completes when the full list is
        /// available, so it may take a while to complete if the collection has a large number of NFTs. For a "lazy-loading"
        /// version of this function, use <see cref="GetNftsForCollectionObservable (string)"/>
        /// </summary>
        /// <param name="contractAddress">The contract address of the collection to query NFTs for</param>
        /// <returns>A task that returns the full list of NFTs in the collection at the given contract address</returns>
        public Task<List<NftItem>> GetNftsForCollection(string contractAddress) =>
            GetNftsForCollectionObservable(contractAddress).AsListAsync();

        /// <summary>
        /// Get all NFTs that are from a specific collection / contract address. This task completes when the full list is
        /// available, so it may take a while to complete if the collection has a large number of NFTs. For a "lazy-loading"
        /// version of this function, use <see cref="GetNftsForCollectionObservable (string)"/>
        /// </summary>
        /// <param name="contractAddress">The contract address of the collection to query NFTs for</param>
        /// <returns>A task that returns the full list of NFTs in the collection at the given contract address</returns>
        public IObservable<NftItem> GetNftsForCollectionObservable(string contractAddress) =>
            ObservablePaginate<NftAssetsResponse, NftItem>($"{ApiPath}/nfts/{contractAddress}/tokens");
        
        /// <summary>
        /// Get the NFT Collection for a given NFT Item.
        /// <see cref="GetCollection (string)"/>
        /// </summary>
        /// <param name="item">The item to grab the NFT Collection from</param>
        /// <returns>The NFT Collection that the given NFT item belongs to</returns>
        public Task<NftCollection> GetCollectionForItem(NftItem item) => GetCollection(item.Contract);

        /// <summary>
        /// Get the NFT Collection at the given contract address
        /// </summary>
        /// <param name="contractAddress">The contract address of the collection to obtain</param>
        /// <returns>NFT Collection data at the given contract address</returns>
        /// <exception cref="ArgumentException">If the contract address given is null or empty</exception>
        public async Task<NftCollection> GetCollection(string contractAddress)
        {
            if (string.IsNullOrWhiteSpace(contractAddress))
                throw new ArgumentException("Invalid account address");

            var apiUrl = $"{ApiPath}/nfts/{contractAddress}";

            var collection = await Get<NftCollection>(apiUrl);

            if (collection == null) return null;
            
            foreach (var org in _orgApiClients.Values)
                await collection.TryLinkOrganization(org);

            return collection;
        }
        #endregion
        
        #region Ownership
        
        /// <summary>
        /// Get a full list of owners of a given NFT collection. This task completes when the full list of owners
        /// is available, so it may take a while if the NFT collection yields a large number of owners. For a "lazy-loading"
        /// version of this function, use <see cref="GetOwnersOfNftCollectionObservable(string)"/>
        /// </summary>
        /// <param name="contractAddress">The contract address of the NFT collection to query owners from</param>
        /// <returns>A task that returns the full list of owners from the specified NFT collection.</returns>
        public Task<List<OwnersResult>> GetOwnersOfNftCollection(string contractAddress) =>
            GetOwnersOfNftCollectionObservable(contractAddress).AsListAsync();
        
        /// <summary>
        /// Get a full list of owners of a given NFT collection. This task completes when the full list of owners
        /// is available, so it may take a while if the NFT collection yields a large number of owners. For a "lazy-loading"
        /// version of this function, use <see cref="GetOwnersOfNftCollectionObservable(string)"/>
        /// </summary>
        /// <param name="collection">The NFT collection to query owners from</param>
        /// <returns>A task that returns the full list of owners from the specified NFT collection.</returns>
        public Task<List<OwnersResult>> GetOwnersOfNftCollection(NftCollection collection) =>
            GetOwnersOfNftCollectionObservable(collection).AsListAsync();
        
        /// <summary>
        /// Get all owners of a given NFT collection. This returns an
        /// observable that emits each owner as it is obtained from the NFT API. This is useful for
        /// "lazy-loading" owners, as the function will complete as soon as the Observable is created and started.
        /// For a "blocking" version of this function, use <see cref="GetOwnersOfNftCollection (string)"/>
        /// </summary>
        /// <param name="contractAddress">The contract address of the NFT collection to query owners from</param>
        /// <returns>An observable that emits owners from the specified NFT collection.
        /// This observable completes when all owners have been emitted.</returns>
        public IObservable<OwnersResult> GetOwnersOfNftCollectionObservable(string contractAddress) =>
            ObservablePaginate<OwnersResponse, OwnersResult>($"{ApiPath}/nfts/{contractAddress}/owners");

        /// <summary>
        /// Get all owners of a given NFT collection. This returns an
        /// observable that emits each owner as it is obtained from the NFT API. This is useful for
        /// "lazy-loading" owners, as the function will complete as soon as the Observable is created and started.
        /// For a "blocking" version of this function, use <see cref="GetOwnersOfNftCollection (string)"/>
        /// </summary>
        /// <param name="collection">The NFT collection to query owners from</param>
        /// <returns>An observable that emits owners from the specified NFT collection.
        /// This observable completes when all owners have been emitted.</returns>
        public IObservable<OwnersResult> GetOwnersOfNftCollectionObservable(NftCollection collection) =>
            GetOwnersOfNftCollectionObservable(collection.Contract);

        /// <summary>
        /// Get a full list of owners of a given NFT. This task completes when the full list of owners
        /// is available, so it may take a while if the NFT yields a large number of owners. For a "lazy-loading"
        /// version of this function, use <see cref="GetOwnersOfNftObservable(string, string)"/>
        /// </summary>
        /// <param name="contractAddress">The contract address of the NFT collection the given tokenId belongs to</param>
        /// <param name="tokenId">The token Id of the NFT to query owners from</param>
        /// <returns>A task that returns the full list of owners from the specified NFT.</returns>
        public Task<List<OwnersResult>> GetOwnersOfNft(string contractAddress, string tokenId) =>
            GetOwnersOfNftObservable(contractAddress, tokenId).AsListAsync();
        
        /// <summary>
        /// Get a full list of owners of a given NFT. This task completes when the full list of owners
        /// is available, so it may take a while if the NFT yields a large number of owners. For a "lazy-loading"
        /// version of this function, use <see cref="GetOwnersOfNftObservable(string, string)"/>
        /// </summary>
        /// <param name="nftItem">The NFT to query owners from</param>
        /// <returns>A task that returns the full list of owners from the specified NFT.</returns>
        public Task<List<OwnersResult>> GetOwnersOfNft(NftItem nftItem) =>
            GetOwnersOfNftObservable(nftItem).AsListAsync();

        /// <summary>
        /// Get all owners of a given NFT. This returns an
        /// observable that emits each owner as it is obtained from the NFT API. This is useful for
        /// "lazy-loading" owners, as the function will complete as soon as the Observable is created and started.
        /// For a "blocking" version of this function, use <see cref="GetOwnersOfNft (string, string)"/>
        /// </summary>
        /// <param name="contractAddress">The contract address of the NFT collection the given tokenId belongs to</param>
        /// <param name="tokenId">The token Id of the NFT to query owners from</param>
        /// <returns>An observable that emits owners from the specified NFT.
        /// This observable completes when all owners have been emitted.</returns>
        public IObservable<OwnersResult> GetOwnersOfNftObservable(string contractAddress, string tokenId) =>
            ObservablePaginate<OwnersResponse, OwnersResult>($"{ApiPath}/nfts/{contractAddress}/{tokenId}/owners");

        /// <summary>
        /// Get all owners of a given NFT. This returns an
        /// observable that emits each owner as it is obtained from the NFT API. This is useful for
        /// "lazy-loading" owners, as the function will complete as soon as the Observable is created and started.
        /// For a "blocking" version of this function, use <see cref="GetOwnersOfNft (string, string)"/>
        /// </summary>
        /// <param name="nftItem">The NFT to query owners from</param>
        /// <returns>An observable that emits owners from the specified NFT.
        /// This observable completes when all owners have been emitted.</returns>
        public IObservable<OwnersResult> GetOwnersOfNftObservable(NftItem nftItem) =>
            GetOwnersOfNftObservable(nftItem.Contract, nftItem.TokenId.ToString());

        #endregion
        
        #region Market Data

        /// <summary>
        /// Get the lowest ETH based price for the given NFT collection.
        /// </summary>
        /// <param name="tokenAddress">The contract address of the NFT collection to get pricing data for</param>
        /// <returns>Token price information for the given NFT collection</returns>
        public Task<TokenPrice> GetTradePrice(string tokenAddress) =>
            Get<TokenPrice>($"{ApiPath}/nfts/{tokenAddress}/tradePrice");

        /// <summary>
        /// Get the lowest ETH based price for the given NFT collection.
        /// </summary>
        /// <param name="collection">The NFT collection to get pricing data for</param>
        /// <returns>Token price information for the given NFT collection</returns>
        public Task<TokenPrice> GetTradePrice(NftCollection collection) => GetTradePrice(collection.Contract);

        /// <summary>
        /// Get the lowest ETH based price for the NFT collection that the given NFT exists inside of.
        /// </summary>
        /// <param name="item">The NFT get pricing data for.</param>
        /// <returns>Token price information for the NFT collection that the given NFT exists inside of.</returns>
        public Task<TokenPrice> GetTradePrice(NftItem item) => GetTradePrice(item.Contract);
        
        #endregion

        #region IPFS
        /// <summary>
        /// Store a file on IPFS and return the CID of the file. The file given can either
        /// be a path to a file in the filesystem or a URL to a remote file.
        /// </summary>
        /// <param name="file">The path or URL of the file to store on IPFS</param>
        /// <returns>The CID of the uploaded file</returns>
        /// <exception cref="Exception">If no IPFS client is setup</exception>
        public Task<string> StoreFile(string file)
        {
            if (IpfsClient == null)
                throw new Exception("IpfsClient not setup");

            return IpfsClient.UploadFile(file);
        }
        
        /// <summary>
        /// Store metadata string on IPFS and return the CID of the file. The given metadata
        /// must be a JSON string.
        /// </summary>
        /// <param name="metadata">The metadata JSON string to store on IPFS</param>
        /// <returns>The CID of the uploaded metadata</returns>
        /// <exception cref="Exception">If no IPFS client is setup</exception>
        public Task<string> StoreMetadata(string metadata)
        {
            if (IpfsClient == null)
                throw new Exception("IpfsClient not setup");

            return IpfsClient.UploadContent(metadata);
        }
        
        /// <summary>
        /// Store metadata as a JSON string on IPFS and return the CID of the file. The given
        /// metadata object will be converted to JSON before being uploaded to IPFS. The type of
        /// Metadata must implement the IMetadata interface
        /// </summary>
        /// <param name="metadata">The metadata object to store on IPFS</param>
        /// <typeparam name="T">The type of the metadata object</typeparam>
        /// <returns>The CID of the uploaded metadata</returns>
        /// <exception cref="Exception">IF no IPFS client is setup</exception>
        public Task<string> StoreMetadata<T>(T metadata) where T : IMetadata
        {
            if (IpfsClient == null)
                throw new Exception("IpfsClient not setup");

            return StoreMetadata(JsonConvert.SerializeObject(metadata));
        }

        /// <summary>
        /// Create a folder of metadata files. This function will create a new folder where
        /// the folder contains each element passed in the given IEnumerable. Each item in the
        /// IEnumerable will be converted to a JSON string before being uploaded. The type of
        /// Metadata must implement the IMetadata interface
        /// </summary>
        /// <param name="metadata">An enumerable list of metadata to place in the new folder</param>
        /// <typeparam name="T">The type of metadata being stored</typeparam>
        /// <returns>The CID of the new folder</returns>
        /// <exception cref="Exception">If no IPFS client is setup</exception>
        public Task<string> CreateFolder<T>(IEnumerable<T> metadata) where T : IMetadata
        {
            if (IpfsClient == null)
                throw new Exception("IpfsClient not setup");

            return CreateFolder(metadata.Select(m => JsonConvert.SerializeObject(m)));
        }
        
        /// <summary>
        /// Create a folder of metadata files. This function will create a new folder where
        /// the folder contains each element passed in the given IEnumerable.
        /// </summary>
        /// <param name="metadata">An enumerable list of strings, where each string will be a new file in the resulting folder</param>
        /// <returns>The CID of the new folder</returns>
        /// <exception cref="Exception">If no IPFS client is setup</exception>
        public Task<string> CreateFolder(IEnumerable<string> metadata)
        {
            if (IpfsClient == null)
                throw new Exception("IpfsClient not setup");

            return IpfsClient.UploadArray(metadata);
        }
        #endregion

        #region Utils
        /// <summary>
        /// Link an organization API key to this client. This will allow the client to access
        /// specific organization data. This will also return an instance of the Organization API
        /// as an <see cref="OrgApiClient"/>
        /// </summary>
        /// <param name="organizationId">The API key of the organization to link</param>
        /// <returns>The Organization API as an <see cref="OrgApiClient"/></returns>
        public OrgApiClient LinkOrganization(string organizationId)
        {
            if (_orgApiClients.ContainsKey(organizationId))
                return _orgApiClients[organizationId];
            
            var orgApi = new OrgApiClient(organizationId, IpfsClient);
            _orgApiClients.Add(organizationId, orgApi);
            return orgApi;
        }
        
        /// <summary>
        /// Update the chain this API Client queries against
        /// </summary>
        /// <param name="chains">The new chain to use for queries</param>
        public void UpdateChain(Chains chains)
        {
            this.Auth.ChainId = chains;
            this.ApiPath = $"/networks/{(int) Auth.ChainId}";
        }
        #endregion

        #region Protected Methods
        protected virtual async Task<T> Get<T>(string fullPath)
        {
            var json = await HttpClient.Get(fullPath);
            var data = JsonConvert.DeserializeObject<T>(json);
            return data;
        }

        protected virtual IObservable<T> ObservablePaginate<TR, T>(string apiUrl)
            where TR : ICursor, IResponseSet<T>
        {
            return ObservablePaginate<TR, T, T>(apiUrl, arg => arg);
        }

        protected virtual IObservable<TS> ObservablePaginate<TR, T, TS>(string apiUrl, Func<T, TS> selector) where TR : ICursor, IResponseSet<T>
        {
            if (selector == null)
                throw new ArgumentException("Selector is null");

            return Observable.Create<TS>(async (observer, cancel) =>
            {
                TR data = default;
                do
                {
                    var cursor = data != null ? $"{(apiUrl.Contains("?") ? "&" : "?")}cursor={data.Cursor}" : "";
                    var fullUrl = $"{apiUrl}{cursor}";

                    data = await Get<TR>(fullUrl);

                    if (data == null) continue;

                    foreach (var item in data.Data)
                    {
                        var result = selector(item);
                        
                        if (result is IOrgLinkable linkable)
                        {
                            foreach (var org in _orgApiClients.Values)
                            {
                                var linkResult = await linkable.TryLinkOrganization(org);
                                if (linkResult)
                                    // We don't need to attempt linking anymore if one Organization was successful
                                    break; 
                            }
                        }
                        
                        observer.OnNext(result);
                    }
                } while (!cancel.IsCancellationRequested && data != null && !string.IsNullOrWhiteSpace(data.Cursor));
            });
        }
        #endregion
    }
}