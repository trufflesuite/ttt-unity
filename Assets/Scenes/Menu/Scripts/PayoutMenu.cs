using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
using MetaMask.NEthereum;
using MetaMask.Unity;
using Newtonsoft.Json;

namespace Scenes.Menu.Scripts
{
    public class PayoutMenu : MonoBehaviour
    {
        public GameObject payoutAmount;
        public GameObject collectButton;
        public GameObject transactionModal;

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
                var ticTacToeAddress = ContractManager.ticTacToeAddress;

                var ticTacToe = new Truffle.Contracts.TicTacToeService(web3, ticTacToeAddress);

                var payments = await ticTacToe.PaymentsQueryAsync(MetaMaskUnity.Instance.Wallet.SelectedAddress);

                if (payments > 0)
                {
                payoutAmount.GetComponent<TMP_Text>().text = payments.ToString("X") + " Wei";
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

    public async void CollectPayments()
        {
            Debug.Log("Collecting payments...");

            GameObject modalHeaderText = transactionModal.transform.Find("Panel/ModalWindow/Header/HeaderText").gameObject;
            modalHeaderText.GetComponent<TMP_Text>().text = "Collecting Payments...";

            transactionModal.SetActive(true);

            var metaMask = MetaMaskUnity.Instance;
            var web3 = metaMask.CreateWeb3();
            var ticTacToeAddress = ContractManager.ticTacToeAddress;

            var ticTacToe = new Truffle.Contracts.TicTacToeService(web3, ticTacToeAddress);

            try
            {
                var transactionHash = await ticTacToe.WithdrawPaymentsRequestAndWaitForReceiptAsync(MetaMaskUnity.Instance.Wallet.SelectedAddress);

                transactionModal.SetActive(false);

                CancelInvoke();
                payoutAmount.GetComponent<TMP_Text>().text = "0 Wei";
                collectButton.SetActive(false);
            }
            catch(Exception e)
            {
                var result = JsonConvert.DeserializeObject<IDictionary<string, int>>(e.Message);

                if(result["code"] == 4001)
                {
                    Debug.Log("Transaction rejected");
                    transactionModal.SetActive(false);
                }
            }
        }
    }
}
