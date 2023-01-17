using System;
using UnityEngine;

namespace Infura.Unity
{
    public class Test : MonoBehaviour
    {
        private InfuraSdk sdk;

        private async void Start()
        {
            sdk = FindObjectOfType<InfuraSdk>();

            await sdk.SdkReadyTask;

            var results = sdk.SelfCustody.SearchNfts("poap");

            results.Subscribe(n => Debug.Log(n.Name));
        }
    }
}