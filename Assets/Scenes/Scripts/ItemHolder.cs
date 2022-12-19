using System;
using System.Threading.Tasks;
using Infura.SDK;
using Infura.Unity.Utils;
using MetaMask.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Scripts
{
    public class ItemHolder : MonoBehaviour
    {
        public ItemData ItemData;
        public TextMeshProUGUI name;
        public Image image;
        public TextMeshProUGUI collectionId;

        private bool minted;
        private Button btn;
        private string txHash;

        private void Start()
        {
            name.text = ItemData.Attributes["title"];
            collectionId.text = ItemData.Id;
            ImageHelper.With(() => image).ShowUrl(ItemData.Media.Image.Full);
            
            btn = GetComponentInChildren<Button>();
            
            if (btn != null)
                btn.onClick.AddListener(ButtonClicked);
        }

        private void ButtonClicked()
        {
            if (!minted)
            {
                MintClick();
            }
            else if (!string.IsNullOrWhiteSpace(txHash))
            {
                Application.OpenURL($"https://goerli.etherscan.io/tx/{txHash}");
            }
        }

        private async void MintClick()
        {
            minted = true;
            try
            {
                btn.interactable = false;
                btn.GetComponentInChildren<TextMeshProUGUI>().text = "Minting...";
                txHash = await ItemData.Mint(MetaMaskUnity.Instance.Wallet.SelectedAddress);

                btn.interactable = true;

                var btnImg = btn.GetComponent<Image>();
                btnImg.color = new Color(0.31f, 0.96f, 0.32f, 1f);
                btn.GetComponentInChildren<TextMeshProUGUI>().text = "View TX";

                await Task.Delay(15000);
                
                btnImg.color = new Color(0.31f, 0.53f, 0.96f, 1f);
                btn.GetComponentInChildren<TextMeshProUGUI>().text = "Mint";
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                minted = false;
            }
        }
    }
}