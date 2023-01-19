using System;
using System.Threading.Tasks;
using Infura.SDK;
using Infura.SDK.Common;
using Infura.SDK.Network;
using Infura.SDK.Organization;
using Infura.Unity.Network;
using UnityEngine;

namespace Infura.Unity
{
    /// <summary>
    /// A Unity component that provides access to the Infura SDK.
    /// </summary>
    [RequireComponent(typeof(UnityHttpService))]
    public class InfuraSdk : MonoBehaviour
    {
        /// <summary>
        /// Options for authenticating with the Infura NFT API.
        /// </summary>
        [Serializable]
        public class InfuraOptionsData
        {
            public string ProjectId;

            public string SecretId;
        }

        /// <summary>
        /// Options for specifying which Chain to query from and how to
        /// interact with that Blockchain.
        /// </summary>
        [Serializable]
        public class GeneralOptions
        {
            public Chains Chain;

            public string RpcUrl;
        }

        /// <summary>
        /// The options to use for authentication
        /// </summary>
        public InfuraOptionsData InfuraOptions;

        /// <summary>
        /// The options to use for IPFS
        /// </summary>
        public IpfsOptions IpfsOptions;

        /// <summary>
        /// The options to use for Blockchain querying and interactions
        /// </summary>
        public GeneralOptions NetworkOptions;

        /// <summary>
        /// Get the IPFS client the SDK is currently using
        /// </summary>
        public SDK.Network.Ipfs Ipfs
        {
            get
            {
                return API.IpfsClient;
            }
        }

        /// <summary>
        /// The Infura NFT API Client. This is the main entry point for the SDK.
        /// </summary>
        public ApiClient API { get; private set; }

        /// <summary>
        /// The Authentication information the SDK is currently using.
        /// </summary>
        public Auth Auth
        {
            get { return API.Auth; }
        }

        private TaskCompletionSource<bool> SdkReadyTaskSource = new TaskCompletionSource<bool>();

        /// <summary>
        /// A Task that can be awaited to determine when the SDK is ready to use.
        /// </summary>
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
            
            var auth = new Auth(InfuraOptions.ProjectId, InfuraOptions.SecretId, NetworkOptions.Chain, NetworkOptions.RpcUrl, IpfsOptions);
            API = new ApiClient(Auth);
            
            SdkReadyTaskSource.SetResult(true);
        }
        
        /// <summary>
        /// Link an Organization to the SDK. This will allow the SDK to interact with the Organization's
        /// API to both gather additional information from the Organization API and to perform actions
        /// using the Organization's API.
        /// </summary>
        /// <param name="orgId">The API Key for the Organization</param>
        /// <returns>The Organization's API as a <see cref="OrgApiClient"/></returns>
        public async Task<OrgApiClient> LinkOrganizationCustody(string orgId)
        {
            await SdkReadyTask;
            return API.LinkOrganization(orgId);
        }
    }
}