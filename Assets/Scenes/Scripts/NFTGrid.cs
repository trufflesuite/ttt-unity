﻿using System;
using GalaxySdk.Utils;
using Infura.SDK;
using Infura.Unity;
using MetaMask.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Scripts
{
    public class NFTGrid : BindableMonoBehavior
    {
        [Inject]
        private InfuraSdk infura;

        public GameObject nftPrefab;

        private void Start()
        {
            UpdateCollections();
        }

        private async void UpdateCollections()
        {
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }

            await infura.SdkReadyTask;

            infura.API.GetNftsObservable(MetaMaskUnity.Instance.Wallet.SelectedAddress)
                .Subscribe(AddNftToGrid);

            /*var nfts = await infura.API.GetNfts(MetaMaskUnity.Instance.Wallet.SelectedAddress);

            foreach (var nft in nfts)
            {
                var collectionInstance = Instantiate(nftPrefab, transform);
                var collectionDataHolder = collectionInstance.GetComponent<NFTHolder>();
                collectionDataHolder.NftData = nft;

                var btn = collectionInstance.GetComponent<Button>();
                if (btn != null)
                    btn.onClick.AddListener(delegate { NFTSelected(nft); });
            }*/
        }

        private void AddNftToGrid(NftItem nft)
        {
            var collectionInstance = Instantiate(nftPrefab, transform);
            var collectionDataHolder = collectionInstance.GetComponent<NFTHolder>();
            collectionDataHolder.NftData = nft;

            var btn = collectionInstance.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(delegate { NFTSelected(nft); });
        }

        private void NFTSelected(NftItem nftData)
        {
            Debug.Log("NFT " + nftData.TokenId + " selected");
        }
    }
}