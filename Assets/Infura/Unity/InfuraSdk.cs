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

        public SdkClient Sdk { get; private set; }
        
        private void Start()
        {
            Sdk = new SdkClient(new Auth(ProjectId, SecretId, RpcUrl, Chain, Ipfs));
        }
    }
}