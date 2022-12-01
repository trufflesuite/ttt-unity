using System;
using Infura.SDK;
using UnityEngine;

namespace Infura.Unity
{
    [RequireComponent(typeof(UnityHttpService))]
    public class InfuraSdk : MonoBehaviour
    {
        public string ProjectId;

        public string SecretId;

        public Chains Chain;

        public string RpcUrl;

        public IpfsOptions Ipfs;

        public ApiClient Api { get; private set; }
        
        public Auth Auth { get; private set; }
        
        private void Start()
        {
            if (string.IsNullOrWhiteSpace(Ipfs.ProjectId))
            {
                Ipfs = null;
                Debug.LogWarning("No IPFS ProjectId set, disabling IPFS");
            }
            else if (string.IsNullOrWhiteSpace(Ipfs.ApiKeySecret))
            {
                Ipfs = null;
                Debug.LogWarning("No IPFS ApiKeySecret set, disabling IPFS");
            }
            
            Auth = new Auth(ProjectId, SecretId, RpcUrl, Chain, Ipfs);
            Api = new ApiClient(Auth);
        }
    }
}