using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using MetaMask.Unity;
using MetaMask.NEthereum;
using Nethereum.JsonRpc.UnityClient;

public class NewGameMenu : MonoBehaviour
{
    public List<string> errors;

    public string jackpotValue;
    public GameObject jackpotInput;

    public string playerXValue;
    public GameObject playerXInput;

    public string playerOValue;
    public GameObject playerOInput;

    public GameObject errorDisplay;

    public async void ValidateForm()
    {
        errors.Clear();

        jackpotValue = jackpotInput.GetComponent<TMP_InputField>().text;
        playerXValue = playerXInput.GetComponent<TMP_InputField>().text;
        playerOValue = playerOInput.GetComponent<TMP_InputField>().text;

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

        // var jackpotBN = new BigInteger(Int32.Parse(jackpotValue));

        var startGameFunction = new Truffle.Functions.StartGameFunction()
        {
            AmountToSend = 100,
            PayoutX = playerXValue,
            PayoutO = playerOValue
        };

        try
        {
            var receipt = await ticTacToe.StartGameRequestAndWaitForReceiptAsync(startGameFunction);
            Debug.Log(receipt);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            throw;
        }

        // string url = "https://metamask-sdk-socket.metafi.codefi.network/";

        // var getLogsRequest = new EthGetLogsUnityRequest(url);
        // var eventGameStarted = Truffle.Data.GameWonEventDTO();
        // yield return getLogsRequest.SendRequest(eventGameStarted.CreateFilterInput());

        // var eventDecoded = getLogsRequest.Result.DecodeAllEvents<Truffle.Data.GameWonEventDTO>();

        // Debug.Log("New game started. Game ID: " + eventDecoded[0].Event.GameId);
    }
}
