using System;
using System.Text;
using Ipfs.Http;
using Nethereum.Web3;

namespace Infura.SDK
{
    public class Auth
    {
        public string ProjectId { get; }
        
        public string SecretId { get; }
        
        public string RpcUrl { get; private set; }
        
        public Chains ChainId { get; }
        
        public Ipfs Ipfs { get; }
        
        public Web3 Provider { get; set; }

        public string ApiAuth
        {
            get
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ProjectId}:{SecretId}"));
            }
        }
        
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

            Ipfs = new Ipfs(ipfs.ProjectId, ipfs.ApiKeySecret);
        }

        public Auth(string projectId, string secretId, string rpcUrl, Chains chainId, Ipfs ipfs = null)
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