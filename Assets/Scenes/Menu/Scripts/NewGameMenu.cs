using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Truffle.Data;
using MetaMask.Unity;
using MetaMask.NEthereum;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class NewGameMenu : MonoBehaviour
{
    public List<string> errors;

    string jackpotValue;
    public GameObject jackpotInput;

    string playerXValue;
    public GameObject playerXInput;

    string playerOValue;
    public GameObject playerOInput;

    public GameObject errorDisplay;

    public void ValidateForm()
    {
        errors.Clear();
        errorDisplay.GetComponent<TMP_Text>().text = string.Empty;

        jackpotValue = jackpotInput.GetComponent<TMP_InputField>().text.Trim();
        playerXValue = playerXInput.GetComponent<TMP_InputField>().text.Trim();
        playerOValue = playerOInput.GetComponent<TMP_InputField>().text.Trim();

        // Regex from: https://regex101.com/r/dF5yhK/1/debugger
        string pattern = "^0x[a-fA-F0-9]{40}$";
        Regex ifEthereumAddress = new Regex(pattern);

        // Existance Checks

        if (string.IsNullOrEmpty(jackpotValue) || jackpotValue == "0")
        {
            errors.Add("Supply a jackpot amount greater than 0.");
        }

        if (string.IsNullOrEmpty(playerXValue))
        {
            errors.Add("Supply a payout address for Player X.");
        }

        if (string.IsNullOrEmpty(playerOValue))
        {
            errors.Add("Supply a payout address for Player O.");
        }

        // Other Checks

        if (!ifEthereumAddress.IsMatch(playerXValue))
        {
            errors.Add("Supply a valid Ethereum address for Player X.");
        }

        if (!ifEthereumAddress.IsMatch(playerOValue))
        {
            errors.Add("Supply a valid Ethereum address for Player O.");
        }

        if (playerXValue == playerOValue)
        {
            errors.Add("Supply a different payout address for Player X or Player O; they cannot be the same.");
        }

        // Display the errors if the exist.

        if (errors.Count > 0)
        {
            DisplayErrors(errors.ToArray());
            return;
        }

        // Otherwise start a new game.

        InitializeGame();
    }

    public void DisplayErrors(string[] errorArray)
    {
        errorDisplay.GetComponent<TMP_Text>().text = string.Join("\n", errorArray);
    }

    public async void InitializeGame()
    {
        // Here we'll call the game smart contract.

        var metaMask = MetaMaskUnity.Instance;
        var web3 = metaMask.CreateWeb3();
        var ticTacToeAddress = "0x72509FD110C1F83c04D9811b8148A5Ba3e1f5FF6";

        var ticTacToe = new Truffle.Contracts.TicTacToeService(web3, ticTacToeAddress);

        var jackpotInt = Convert.ToInt32(jackpotValue);
        var jackpotBN = new HexBigInteger(jackpotInt);

        // We create the StartGameFunction object so we can attach ETH via AmountToSend.

        var startGameFunction = new Truffle.Functions.StartGameFunction();
            startGameFunction.AmountToSend = jackpotBN;
            startGameFunction.PayoutX = playerXValue;
            startGameFunction.PayoutO = playerOValue;

        var receipt = await ticTacToe.StartGameRequestAndWaitForReceiptAsync(startGameFunction);
        int gameId = Convert.ToInt32(receipt.Logs[0]["data"].ToString(), 16);

        PlayerPrefs.SetInt("gameId", gameId);
        PlayerPrefs.SetInt("jackpot", jackpotInt);
        PlayerPrefs.SetString("playerX", playerXValue);
        PlayerPrefs.SetString("playerO", playerOValue);

        SceneManager.LoadScene("TicTacToe");
    }
}
