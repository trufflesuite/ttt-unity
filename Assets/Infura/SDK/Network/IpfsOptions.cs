using System;
using UnityEngine;

namespace Infura.SDK.Network
{
    [Serializable]
    public class IpfsOptions
    {
        [SerializeField]
        private string projectId;

        [SerializeField]
        private string apiKeySecret;

        /// <summary>
        /// The project ID of your Infura project to use for IPFS API requests.
        /// </summary>
        public string ProjectId
        {
            get
            {
                return projectId;
            }
            set
            {
                projectId = value;
            }
        }

        /// <summary>
        /// The project secret of your Infura project to use for IPFS API requests.
        /// </summary>
        public string ApiKeySecret
        {
            get
            {
                return apiKeySecret;
            }
            set
            {
                apiKeySecret = value;
            }
        }

        /// <summary>
        /// Create new IPFS options with no project ID or secret.
        /// </summary>
        public IpfsOptions()
        {
        }

        /// <summary>
        /// Create new IPFS options with the given project ID and secret.
        /// </summary>
        /// <param name="projectId">The project Id to use</param>
        /// <param name="apiKeySecret">The project secret to use</param>
        public IpfsOptions(string projectId, string apiKeySecret)
        {
            ProjectId = projectId;
            ApiKeySecret = apiKeySecret;
        }
    }
}