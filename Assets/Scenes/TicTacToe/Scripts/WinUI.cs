using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MetaMask.NEthereum;
using MetaMask.Unity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;

public class WinUI : MonoBehaviour {
   [Header ("UI References :")]
   [SerializeField] private GameObject uiCanvas;
   [SerializeField] private Text uiWinnerText;
   [SerializeField] private Button uiRematchButton;
   [SerializeField] private Button uiMainMenuButton;
   [SerializeField] private Button uiReleaseJackpotButton;
   [SerializeField] private Text uiGameId;
   [SerializeField] private Text uiJackpot;
   [SerializeField] private GameObject transactionModal;

   [Header ("Board Reference :")]
   [SerializeField] private Board board;

   private void Start() {
      uiRematchButton.onClick.AddListener(() => SceneManager.LoadScene("TicTacToe"));
      board.OnWinAction += OnWinEvent;

      int gameId = PlayerPrefs.GetInt("gameId");
      int jackpot = PlayerPrefs.GetInt("jackpot");
      string playerX = PlayerPrefs.GetString("playerX");
      string playerO = PlayerPrefs.GetString("playerO");

      uiGameId.text = "Game ID: " + gameId;
      uiJackpot.text = "Jackpot: " + jackpot + " Wei";

      uiCanvas.SetActive(false);
   }

   private void OnWinEvent(Mark mark, Color color) {
      int jackpot = PlayerPrefs.GetInt("jackpot");

      if (mark == Mark.None)
      {
         // Nobody wins
         uiWinnerText.text = "Nobody wins. Try again!";
         uiRematchButton.gameObject.SetActive(true);
      }
      else
      {
        uiWinnerText.text = mark.ToString() + " wins! Release their " + jackpot + " Wei jackpot to start a new game.";
        uiReleaseJackpotButton.onClick.AddListener(() => ReleaseJackpot(mark));
        uiReleaseJackpotButton.gameObject.SetActive(true);
      }

      uiWinnerText.color = color;

      uiCanvas.SetActive(true);
   }

   private async void ReleaseJackpot(Mark mark) {
      int gameId = PlayerPrefs.GetInt("gameId");
      int jackpot = PlayerPrefs.GetInt("jackpot");

      GameObject modalHeaderText = transactionModal.transform.Find("Panel/ModalWindow/Header/HeaderText").gameObject;
      modalHeaderText.GetComponent<TMP_Text>().text = "Ending Game...";

      transactionModal.SetActive(true);

      int winner = mark == Mark.X ? 0 : 1;

      var metaMask = MetaMaskUnity.Instance;
      var web3 = metaMask.CreateWeb3();
      var ticTacToeAddress = "0x72509FD110C1F83c04D9811b8148A5Ba3e1f5FF6";

      var ticTacToe = new Truffle.Contracts.TicTacToeService(web3, ticTacToeAddress);

      var gameIdBN = new BigInteger(gameId);
      var winnerBN = new BigInteger(winner);

      try
      {
         var receipt = await ticTacToe.EndGameRequestAndWaitForReceiptAsync(gameIdBN, winnerBN);

         PlayerPrefs.DeleteKey("gameId");
         PlayerPrefs.DeleteKey("jackpot");
         PlayerPrefs.DeleteKey("playerX");
         PlayerPrefs.DeleteKey("playerO");

         SceneManager.LoadScene("Menu");
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

   private void OnDestroy() {
      uiRematchButton.onClick.RemoveAllListeners();
      uiReleaseJackpotButton.onClick.RemoveAllListeners();
      board.OnWinAction -= OnWinEvent;
   }
}
