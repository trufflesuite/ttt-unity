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
        
        public CNFTClient OrganizationCustody { get; private set; }

        public SDK.Ipfs Ipfs
        {
            get
            {
                return SelfCustody.IpfsClient;
            }
        }

        public ApiClient SelfCustody { get; private set; }
        
        public Auth Auth { get; private set; }
        
        private async void Start()
        {
            if (string.IsNullOrWhiteSpace(IpfsOptions.ProjectId))
            {
                IpfsOptions = null;
                Debug.LogWarning("No IPFS ProjectId set, disabling IPFS");
            }
            else if (string.IsNullOrWhiteSpace(IpfsOptions.ApiKeySecret))
            {
                IpfsOptions = null;
                Debug.LogWarning("No IPFS ApiKeySecret set, disabling IPFS");
            }
            
            Auth = new Auth(InfuraOptions.ProjectId, InfuraOptions.SecretId, NetworkOptions.RpcUrl, NetworkOptions.Chain, IpfsOptions);
            SelfCustody = new ApiClient(Auth);
            OrganizationCustody = new CNFTClient(OrganizationOptions.ApiKey, Auth.Ipfs);
        }
    }
}