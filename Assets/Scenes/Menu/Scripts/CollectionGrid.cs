using GalaxySdk.Utils;
using Infura.SDK.Organization;
using Infura.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Scripts
{
    public class CollectionGrid : BindableMonoBehavior
    {
        [Inject]
        private InfuraSdk infura;

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

            var org = await infura.LinkOrganizationCustody(organizationApiKey);
            var collections = await org.GetAllCollections();

            //var value = await infura.API.GetCollection(collections[0].Contract.Address);
            
            
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