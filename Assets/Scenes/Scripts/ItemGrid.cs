using System;
using System.Threading.Tasks;
using GalaxySdk.Utils;
using Infura.SDK;
using Infura.Unity;
using MetaMask.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Scripts
{
    public class ItemGrid : BindableMonoBehavior
    {
        public GameObject itemPrefab;

        public CollectionData selectedCollection;

        private void OnEnable()
        {
            UpdateItems();
        }

        public async void UpdateItems()
        {
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }

            var items = await selectedCollection.GetItems();

            foreach (var item in items)
            {
                var itemInstance = Instantiate(itemPrefab, transform);
                var itemDataHolder = itemInstance.GetComponent<ItemHolder>();
                itemDataHolder.ItemData = item;
            }
        }
    }
}