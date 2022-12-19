using System;
using GalaxySdk.Utils;
using MetaMask.Unity;
using TMPro;

namespace Scenes.Scripts
{
    public class AddressText : BindableMonoBehavior
    {
        [BindComponent]
        private TextMeshProUGUI _addressText;

        private void FixedUpdate()
        {
            if (MetaMaskUnity.Instance != null && MetaMaskUnity.Instance.Wallet != null &&
                !string.IsNullOrWhiteSpace(MetaMaskUnity.Instance.Wallet.SelectedAddress))
                _addressText.text = MetaMaskUnity.Instance.Wallet.SelectedAddress;
            else
                _addressText.text = "";
        }
    }
}