using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Truffle.Data;
using MetaMask.Unity;
using MetaMask.NEthereum;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.Web3;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class NewGameMenu : MonoBehaviour
{
    public bool errors;

    private string jackpotValue;
    public GameObject jackpotInput;
    public GameObject jackpotErrorDisplay;

    private string playerXValue;
    public GameObject playerXInput;
    public GameObject playerXErrorDisplay;

    private string playerOValue;
    public GameObject playerOInput;
    public GameObject playerOErrorDisplay;

    public GameObject transactionModal;

    public void ValidateForm()
    {
        ResetErrors();

        jackpotValue = jackpotInput.GetComponent<TMP_InputField>().text.Trim();
        playerXValue = playerXInput.GetComponent<TMP_InputField>().text.Trim();
        playerOValue = playerOInput.GetComponent<TMP_InputField>().text.Trim();

        ValidateJackpot();
        ValidatePlayerXAddress();
        ValidatePlayerOAddress();

        // If there are any errors, return.

        if(errors)
        {
            return;
        }

        // Otherwise start a new game.

        InitializeGame();
    }

    public void ValidateJackpot()
    {
        if (string.IsNullOrEmpty(jackpotValue) || jackpotValue == "0")
        {
            jackpotInput.GetComponent<Outline>().enabled = true;
            jackpotErrorDisplay.GetComponent<TMP_Text>().text = "Supply a jackpot amount greater than 0.";
            errors = true;
        }
    }

    public void ValidatePlayerXAddress()
    {
        if (string.IsNullOrEmpty(playerXValue))
        {
            playerXInput.GetComponent<Outline>().enabled = true;
            playerXErrorDisplay.GetComponent<TMP_Text>().text = "Supply a payout address for Player X.";
            errors = true;
            return;
        }

        string pattern = "^0x[a-fA-F0-9]{40}$";
        Regex ifEthereumAddress = new Regex(pattern);

        if (!ifEthereumAddress.IsMatch(playerXValue))
        {
            playerXInput.GetComponent<Outline>().enabled = true;
            playerXErrorDisplay.GetComponent<TMP_Text>().text = "Supply a valid Ethereum address for Player X.";
            errors = true;
        }
    }

    public void ValidatePlayerOAddress()
    {
        if (string.IsNullOrEmpty(playerOValue))
        {
            playerOInput.GetComponent<Outline>().enabled = true;
            playerOErrorDisplay.GetComponent<TMP_Text>().text = "Supply a payout address for Player O.";
            errors = true;
            return;
        }

        string pattern = "^0x[a-fA-F0-9]{40}$";
        Regex ifEthereumAddress = new Regex(pattern);

        if (!ifEthereumAddress.IsMatch(playerOValue))
        {
            playerOInput.GetComponent<Outline>().enabled = true;
            playerOErrorDisplay.GetComponent<TMP_Text>().text = "Supply a valid Ethereum address for Player O.";
            errors = true;
            return;
        }

        if (playerXValue == playerOValue)
        {
            playerOInput.GetComponent<Outline>().enabled = true;
            playerOErrorDisplay.GetComponent<TMP_Text>().text = "Player X address cannot match Player O.";
            errors = true;
        }
    }

    private void ResetErrors()
    {
        errors = false;

        jackpotInput.GetComponent<Outline>().enabled = false;
        jackpotErrorDisplay.GetComponent<TMP_Text>().text = "";

        playerXInput.GetComponent<Outline>().enabled = false;
        playerXErrorDisplay.GetComponent<TMP_Text>().text = "";

        playerOInput.GetComponent<Outline>().enabled = false;
        playerOErrorDisplay.GetComponent<TMP_Text>().text = "";
    }

    public async void InitializeGame()
    {
        GameObject modalHeaderText = transactionModal.transform.Find("Panel/ModalWindow/Header/HeaderText").gameObject;
        modalHeaderText.GetComponent<TMP_Text>().text = "Starting New Game...";

        transactionModal.SetActive(true);

        // Here we'll call the game smart contract.

        var metaMask = MetaMaskUnity.Instance;
        var web3 = metaMask.CreateWeb3();
        var ticTacToeAddress = "0x72509FD110C1F83c04D9811b8148A5Ba3e1f5FF6";

        var ticTacToe = new Truffle.Contracts.TicTacToeService(web3, ticTacToeAddress);

        var jackpotInt = Convert.ToInt32(jackpotValue);

        // We create the StartGameFunction object so we can attach ETH via AmountToSend.

        var startGameFunction = new Truffle.Functions.StartGameFunction();
            startGameFunction.AmountToSend = jackpotInt;
            startGameFunction.PayoutX = playerXValue;
            startGameFunction.PayoutO = playerOValue;

        try
        {
            var receipt = await ticTacToe.StartGameRequestAndWaitForReceiptAsync(startGameFunction);
            int gameId = Convert.ToInt32(receipt.Logs[0]["data"].ToString(), 16);

            PlayerPrefs.SetInt("gameId", gameId);
            PlayerPrefs.SetInt("jackpot", jackpotInt);
            PlayerPrefs.SetString("playerX", playerXValue);
            PlayerPrefs.SetString("playerO", playerOValue);

            SceneManager.LoadScene("TicTacToe");
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
