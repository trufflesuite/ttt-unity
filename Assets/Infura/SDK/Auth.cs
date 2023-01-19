using System;
using System.Text;
using Infura.SDK.Common;
using Infura.SDK.Network;
using Nethereum.Web3;

namespace Infura.SDK
{
    /// <summary>
    /// A class containing authentication information for the Infura API.
    /// </summary>
    public class Auth
    {
        /// <summary>
        /// The project Id to use for authentication.
        /// </summary>
        public string ProjectId { get; }
        
        /// <summary>
        /// The project secret to use for authentication.
        /// </summary>
        public string SecretId { get; }
        
        /// <summary>
        /// The RPC URL to use for on-chain interactions
        /// </summary>
        public string RpcUrl { get; private set; }
        
        /// <summary>
        /// The chain to use for queries in the Infura NFT API.
        /// </summary>
        public Chains ChainId { get; set; }
        
        /// <summary>
        /// The IPFS client to use for IPFS interactions.
        /// </summary>
        public Network.Ipfs Ipfs { get; }
        
        /// <summary>
        /// The web3 client to use for on-chain interactions.
        /// </summary>
        public Web3 Provider { get; set; }

        /// <summary>
        /// The raw API key to use for authentication. This will be used in the backing HTTP Client
        /// </summary>
        public string ApiAuth
        {
            get
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ProjectId}:{SecretId}"));
            }
        }
        
        /// <summary>
        /// Create a new instance of the Auth class.
        /// </summary>
        /// <param name="projectId">The project Id to use for authentication</param>
        /// <param name="secretId">The project secret to use for authentication</param>
        /// <param name="chainId">The chain to use for queries</param>
        /// <param name="rpcUrl">The RPC URL to use for on-chain interactions (optional)</param>
        /// <param name="ipfs">The IPFS client to use for IPFS interactions (optional)</param>
        /// <param name="provider">The web3 provider to use for on-chain interactions (optional)</param>
        /// <exception cref="ArgumentException">If the project Id or project secret are null or empty</exception>
        public Auth(string projectId, string secretId, Chains chainId, string rpcUrl = null, IpfsOptions ipfs = null, Web3 provider = null)
        {
            ProjectId = projectId;
            SecretId = secretId;
            RpcUrl = rpcUrl;
            ChainId = chainId;
            
            ValidateRpcUrl();

            if (ipfs == null) return;
            
            if (string.IsNullOrWhiteSpace(ipfs.ProjectId))
                throw new ArgumentException("Expected IPFS Project Id");

            if (string.IsNullOrWhiteSpace(ipfs.ApiKeySecret))
                throw new ArgumentException("Expected IPFS API Key Secret");

            Ipfs = new Network.Ipfs(ipfs.ProjectId, ipfs.ApiKeySecret);
        }

        /// <summary>
        /// Create a new instance of the Auth class.
        /// </summary>
        /// <param name="projectId">The project Id to use for authentication</param>
        /// <param name="secretId">The project secret to use for authentication</param>
        /// <param name="chainId">The chain to use for queries</param>
        /// <param name="rpcUrl">The RPC URL to use for on-chain interactions (optional)</param>
        /// <param name="ipfs">The IPFS client to use for IPFS interactions (optional)</param>
        /// <exception cref="ArgumentException">If the project Id or project secret are null or empty</exception>
        public Auth(string projectId, string secretId, Chains chainId, string rpcUrl = null, Network.Ipfs ipfs = null)
        {
            ProjectId = projectId;
            SecretId = secretId;
            RpcUrl = rpcUrl;
            ChainId = chainId;
            Ipfs = ipfs;
            
            ValidateRpcUrl();
        }

        private void ValidateRpcUrl()
        {
            if (string.IsNullOrWhiteSpace(RpcUrl))
            {
                RpcUrl = $"{ChainId.RpcUrl()}/v3/{ProjectId}";
            }
        }
    }
}