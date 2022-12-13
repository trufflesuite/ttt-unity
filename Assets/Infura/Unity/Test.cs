using System;
using UnityEngine;

namespace Infura.Unity
{
    public class Test : MonoBehaviour
    {
        private InfuraSdk sdk;

        private void Start()
        {
            sdk = FindObjectOfType<InfuraSdk>();
        }

        private async void OnWinLevel()
        {
            var c = await sdk.OrganizationCustody.GetAllCollections();
        }
    }
}