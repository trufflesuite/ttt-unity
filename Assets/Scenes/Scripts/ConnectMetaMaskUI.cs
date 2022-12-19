using System;
using MetaMask.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Scripts
{
    public class ConnectMetaMaskUI : MonoBehaviour
    {
        public GameObject nextUI;
        public GameObject overlay;

        private void Start()
        {
            HideOverlay();
        }

        private void HideOverlay()
        {
            var img = overlay.GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
            img.raycastTarget = false;

            var rimg = overlay.GetComponentInChildren<RawImage>();
            rimg.color = new Color(rimg.color.r, rimg.color.g, rimg.color.b, 0f);
            rimg.raycastTarget = false;
        }
        
        private void ShowOverlay()
        {
            var img = overlay.GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.35f);
            img.raycastTarget = true;

            var rimg = overlay.GetComponentInChildren<RawImage>();
            rimg.color = new Color(rimg.color.r, rimg.color.g, rimg.color.b, 1f);
            rimg.raycastTarget = true;
        }

        public void Connect()
        {
            ShowOverlay();
            
            MetaMaskUnity.Instance.Wallet.WalletAuthorized += OnWalletAuthorized;
            
            MetaMaskUnity.Instance.Connect();
        }

        private void OnWalletAuthorized(object sender, EventArgs e)
        {
            HideOverlay();
            nextUI.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}