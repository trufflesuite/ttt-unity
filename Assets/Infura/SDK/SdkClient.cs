using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Infura.SDK
{
    public class SdkClient
    {
        public const string NFT_API_URL = "https://nft.api.infura.io";
        
        public Auth Auth { get; }
        
        public string ApiPath { get; }
        
        public IHttpService HttpClient { get; }
        
        public Ipfs IpfsClient { get; }

        public SdkClient(Auth auth)
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
    }
}