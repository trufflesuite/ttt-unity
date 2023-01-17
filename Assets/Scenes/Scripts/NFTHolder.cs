using System;
using Infura.SDK;
using Infura.SDK.SelfCustody;
using Infura.Unity.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Scripts
{
    public class NFTHolder : MonoBehaviour
    {
        public NftItem NftData;
        public TextMeshProUGUI name;
        public Image image;
        public TextMeshProUGUI collectionId;

        private void Start()
        {
            name.text = NftData.TokenId.ToString();
            collectionId.text = NftData.Contract;
            ImageHelper.With(() => image).ShowUrl(NftData.ImageUrl);
        }
    }
}