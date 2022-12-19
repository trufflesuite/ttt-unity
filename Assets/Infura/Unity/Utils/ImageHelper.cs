using System;
using UnityEngine;
using UnityEngine.UI;

namespace Infura.Unity.Utils
{
    public static class ImageHelper
    {
        public class ImageAttachment
        {
            internal Func<Image> imageFunc;

            public void ShowUrl(string url)
            {
                ImageDownloadManager.Instance.EnqueueRequest(url, tex =>
                {
                    var image = imageFunc();
                    image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                });
            }
        }

        public static ImageAttachment With(Func<Image> func)
        {
            return new ImageAttachment()
            {
                imageFunc = func
            };
        }
    }
}