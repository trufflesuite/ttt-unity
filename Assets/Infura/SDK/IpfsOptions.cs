using System;
using UnityEngine;

namespace Infura.SDK
{
    [Serializable]
    public class IpfsOptions
    {
        [SerializeField]
        private string projectId;

        [SerializeField]
        private string apiKeySecret;

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

        public IpfsOptions()
        {
        }

        public IpfsOptions(string projectId, string apiKeySecret)
        {
            ProjectId = projectId;
            ApiKeySecret = apiKeySecret;
        }
    }
}