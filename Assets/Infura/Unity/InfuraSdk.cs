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
        
        private void Start()
        {
            Api = new ApiClient(new Auth(ProjectId, SecretId, RpcUrl, Chain, Ipfs));
        }
    }
}