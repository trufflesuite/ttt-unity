using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MetaMask.NEthereum;
using MetaMask.Unity;
using System.Numerics;

public class WinUI : MonoBehaviour {
   [Header ("UI References :")]
   [SerializeField] private GameObject uiCanvas;
   [SerializeField] private Text uiWinnerText;
   [SerializeField] private Button uiRematchButton;
   [SerializeField] private Button uiMainMenuButton;
   [SerializeField] private Text uiGameId;
   [SerializeField] private Text uiJackpot;

   [Header ("Board Reference :")]
   [SerializeField] private Board board;

   private void Start() {
      uiRematchButton.onClick.AddListener(() => SceneManager.LoadScene("TicTacToe"));
      uiMainMenuButton.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
      board.OnWinAction += OnWinEvent;

      int gameId = PlayerPrefs.GetInt("gameId");
      int jackpot = PlayerPrefs.GetInt("jackpot");
      string playerX = PlayerPrefs.GetString("playerX");
      string playerO = PlayerPrefs.GetString("playerO");

      uiGameId.text = "Game ID: " + gameId;
      uiJackpot.text = "Jackpot: " + jackpot + " Wei";

      uiCanvas.SetActive(false);
   }

   private async void OnWinEvent(Mark mark, Color color) {
      int gameId = PlayerPrefs.GetInt("gameId");
      int jackpot = PlayerPrefs.GetInt("jackpot");

      if (mark == Mark.None)
      {
         // Nobody wins
         uiWinnerText.text = "Nobody wins. Try again!";
         uiRematchButton.gameObject.SetActive(true);
      }
      else
      {
        int winner = mark == Mark.X ? 0 : 1;

        var metaMask = MetaMaskUnity.Instance;
        var web3 = metaMask.CreateWeb3();
        var ticTacToeAddress = "0x72509FD110C1F83c04D9811b8148A5Ba3e1f5FF6";

        var ticTacToe = new Truffle.Contracts.TicTacToeService(web3, ticTacToeAddress);

        var gameIdBN = new BigInteger(gameId);
        var winnerBN = new BigInteger(winner);

        var receipt = await ticTacToe.EndGameRequestAndWaitForReceiptAsync(gameIdBN, winnerBN);

        PlayerPrefs.DeleteKey("gameId");
        PlayerPrefs.DeleteKey("jackpot");
        PlayerPrefs.DeleteKey("playerX");
        PlayerPrefs.DeleteKey("playerO");

        uiWinnerText.text = mark.ToString() + "wins" + jackpot + "Wei!";
        uiMainMenuButton.gameObject.SetActive(true);
      }

      uiWinnerText.color = color;

      uiCanvas.SetActive(true);
   }

   private void OnDestroy() {
      uiRematchButton.onClick.RemoveAllListeners();
      uiMainMenuButton.onClick.RemoveAllListeners();
      board.OnWinAction -= OnWinEvent;
   }
}
