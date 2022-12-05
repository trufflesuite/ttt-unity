using System;
using Infura.SDK;
using UnityEngine;

namespace Infura.Unity
{
    [RequireComponent(typeof(UnityHttpService))]
    public class InfuraSdk : MonoBehaviour
    {
        [Serializable]
        public class InfuraOptionsData
        {
            public string ProjectId;

            public string SecretId;
        }
        
        [Serializable]
        public class CNFTOptions
        {
            public string ApiKey;
        }

        [Serializable]
        public class GeneralOptions
        {
            public Chains Chain;

            public string RpcUrl;
        }

        public InfuraOptionsData InfuraOptions;

        public CNFTOptions OrganizationOptions;
        
        public IpfsOptions IpfsOptions;

        public GeneralOptions NetworkOptions;
        
        public CNFT Organization { get; private set; }
        
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
            
            Auth = new Auth(Infura.ProjectId, Infura.SecretId, NetworkOptions.RpcUrl, NetworkOptions.Chain, Ipfs);
            Api = new ApiClient(Auth);
        }
    }
}