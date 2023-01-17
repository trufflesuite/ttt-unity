using System;
using System.Text;
using Infura.SDK.Common;
using Infura.SDK.Network;
using Nethereum.Web3;

namespace Infura.SDK.SelfCustody
{
    /// <summary>
    /// 
    /// </summary>
    public class Auth
    {
        /// <summary>
        /// 
        /// </summary>
        public string ProjectId { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public string SecretId { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public string RpcUrl { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public Chains ChainId { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public Network.Ipfs Ipfs { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public Web3 Provider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApiAuth
        {
            get
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ProjectId}:{SecretId}"));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="secretId"></param>
        /// <param name="rpcUrl"></param>
        /// <param name="chainId"></param>
        /// <param name="ipfs"></param>
        /// <param name="provider"></param>
        /// <exception cref="ArgumentException"></exception>
        public Auth(string projectId, string secretId, string rpcUrl, Chains chainId, IpfsOptions ipfs = null, Web3 provider = null)
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
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="secretId"></param>
        /// <param name="rpcUrl"></param>
        /// <param name="chainId"></param>
        /// <param name="ipfs"></param>
        public Auth(string projectId, string secretId, string rpcUrl, Chains chainId, Network.Ipfs ipfs = null)
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