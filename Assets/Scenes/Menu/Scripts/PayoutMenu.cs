using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MetaMask.NEthereum;
using MetaMask.Unity;

namespace Scenes.Menu.Scripts
{
    public class PayoutMenu : MonoBehaviour
    {
        public GameObject payoutAmount;
        public GameObject collectButton;

        private void Start()
        {
            collectButton.SetActive(false);

            InvokeRepeating("CheckPayouts", 0, 5);
        }

        private async void CheckPayouts()
        {
            Debug.Log("Checking for payouts...");

            if (MetaMaskUnity.Instance != null && MetaMaskUnity.Instance.Wallet != null && !string.IsNullOrWhiteSpace(MetaMaskUnity.Instance.Wallet.SelectedAddress))
            {
                var metaMask = MetaMaskUnity.Instance;
                var web3 = metaMask.CreateWeb3();
                var ticTacToeAddress = "0x72509FD110C1F83c04D9811b8148A5Ba3e1f5FF6";

                var ticTacToe = new Truffle.Contracts.TicTacToeService(web3, ticTacToeAddress);

                var payments = await ticTacToe.PaymentsQueryAsync(MetaMaskUnity.Instance.Wallet.SelectedAddress);

                if (payments > 0)
                {
                    payoutAmount.GetComponent<TMP_Text>().text = payments.ToString() + " Wei";
                    collectButton.SetActive(true);
                }
                else
                {
                    payoutAmount.GetComponent<TMP_Text>().text = "0 Wei";
                    collectButton.SetActive(false);
                }
            }
            else
            {
                payoutAmount.GetComponent<TMP_Text>().text = "0 Wei";
            }
        }
    }
}
