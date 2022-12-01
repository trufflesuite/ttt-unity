namespace Infura.SDK
{
    public class IpfsOptions
    {
        public string ProjectId { get; set; }
        
        public string ApiKeySecret { get; set; }

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