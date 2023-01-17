using System;
using System.Threading.Tasks;
using Infura.SDK.Common;
using Infura.SDK.Network;
using Infura.SDK.Organization;
using Infura.SDK.SelfCustody;
using Infura.Unity.Network;
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
        public class GeneralOptions
        {
            public Chains Chain;

            public string RpcUrl;
        }

        public InfuraOptionsData InfuraOptions;

        public IpfsOptions IpfsOptions;

        public GeneralOptions NetworkOptions;

        public SDK.Network.Ipfs Ipfs
        {
            get
            {
                return SelfCustody.IpfsClient;
            }
        }

        public ApiClient SelfCustody { get; private set; }
        
        public Auth Auth { get; private set; }

        private TaskCompletionSource<bool> SdkReadyTaskSource = new TaskCompletionSource<bool>();

        public Task SdkReadyTask
        {
            get
            {
                return SdkReadyTaskSource.Task;
            }
        }

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
            
            SdkReadyTaskSource.SetResult(true);
        }
        
        public async Task<OrgApiClient> GetOrganizationCustody(string orgId)
        {
            await SdkReadyTask;
            return new OrgApiClient(orgId, Auth.Ipfs);
        }
    }
}