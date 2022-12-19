using System;
using Infura.SDK;
using Infura.Unity.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Scripts
{
    public class CollectionHolder : MonoBehaviour
    {
        public CollectionData CollectionData;
        public TextMeshProUGUI name;
        public Image image;
        public TextMeshProUGUI collectionId;

        private void Start()
        {
            name.text = CollectionData.Name;
            collectionId.text = CollectionData.Id;
            ImageHelper.With(() => image).ShowUrl(CollectionData.ImageUrl);
        }
    }
}