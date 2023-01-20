using System;
using UnityEngine;

namespace Infura.Unity
{
    public class Test : MonoBehaviour
    {
        private InfuraSdk infura;

        private async void Start()
        {
            infura = FindObjectOfType<InfuraSdk>();

            await infura.SdkReadyTask;

            var results = infura.API.SearchNftsObservable("poap");

            results.Subscribe(n => Debug.Log(n.Name));
        }
    }
}