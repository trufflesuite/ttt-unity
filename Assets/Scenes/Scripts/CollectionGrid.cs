using System;
using GalaxySdk.Utils;
using Infura.SDK;
using Infura.Unity;
using Infura.Unity.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Scenes.Scripts
{
    public class CollectionGrid : BindableMonoBehavior
    {
        [Inject]
        private InfuraSdk sdk;

        public ItemGrid itemGrid;
        public GameObject collectionPrefab;

        private void Start()
        {
            UpdateCollections();
        }

        private async void UpdateCollections()
        {
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }

            await sdk.SdkReadyTask;
            
            var collections = await sdk.OrganizationCustody.GetAllCollections();

            foreach (var collection in collections)
            {
                var collectionInstance = Instantiate(collectionPrefab, transform);
                var collectionDataHolder = collectionInstance.GetComponent<CollectionHolder>();
                collectionDataHolder.CollectionData = collection;

                var btn = collectionInstance.GetComponent<Button>();
                if (btn != null)
                    btn.onClick.AddListener(delegate { CollectionSelected(collection); });
            }
        }

        private void CollectionSelected(CollectionData collectionData)
        {
            itemGrid.selectedCollection = collectionData;
            itemGrid.GetComponentInParent<Canvas>(true).gameObject.SetActive(true);
            GetComponentInParent<Canvas>().gameObject.SetActive(false);
        }
    }
}