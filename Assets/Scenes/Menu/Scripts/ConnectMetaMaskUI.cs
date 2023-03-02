using System;
using MetaMask.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Menu.Scripts
{
    public class ConnectMetaMaskUI : MonoBehaviour
    {
        public GameObject nextUI;
        public GameObject addressContainer;
        public GameObject payoutContainer;

        private void Start()
        {
            Connect();
        }

        public void Connect()
        {
            MetaMaskUnity.Instance.Wallet.WalletAuthorized += OnWalletAuthorized;
            
            MetaMaskUnity.Instance.Connect();
        }

        private void OnWalletAuthorized(object sender, EventArgs e)
        {
            Debug.Log(MetaMaskUnity.Instance.Wallet.SelectedAddress);
            
            addressContainer.gameObject.SetActive(true);
            payoutContainer.gameObject.SetActive(true);
            nextUI.gameObject.SetActive(true);
            
            gameObject.SetActive(false);
        }
    }
}