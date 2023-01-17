using Newtonsoft.Json;

namespace Infura.SDK.Organization
{
    public class ItemDataMedia
    {
        public class ImageData
        {
            [JsonProperty("cid")]
            public string Cid { get; set; }
            
            [JsonProperty("full")]
            public string Full { get; set; }
            
            [JsonProperty("original")]
            public string Original { get; set; }
            
            [JsonProperty("thumb")]
            public string Thumbnail { get; set; }
            
            [JsonProperty("tiny")]
            public string Tiny { get; set; }
        }
        
        [JsonProperty("image")]
        public ImageData Image { get; set; }
    }
}