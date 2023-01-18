using System;
using GalaxySdk.Utils;
using Infura.SDK;
using Infura.SDK.Organization;
using Infura.Unity;
using Infura.Unity.Utils;
using MetaMask.Unity;
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

        private OrgApiClient org;

        public ItemGrid itemGrid;
        public GameObject collectionPrefab;
        public string organizationApiKey;

        private void Start()
        {
            UpdateCollections();
        }

        private async void UpdateCollections()
        {
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }

            var org = await sdk.LinkOrganizationCustody(organizationApiKey);
            var collections = await org.GetAllCollections();

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