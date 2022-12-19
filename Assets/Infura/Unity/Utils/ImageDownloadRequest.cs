using UnityEngine;
using UnityEngine.Events;

namespace Infura.Unity.Utils
{
    public struct ImageDownloadRequest
    {
        public string URL { get; }
        
        public UnityAction<Texture2D> OnImageDownloaded { get; }

        public ImageDownloadRequest(string url, UnityAction<Texture2D> onImageDownloaded)
        {
            URL = url;
            OnImageDownloaded = onImageDownloaded;
        }
    }
}