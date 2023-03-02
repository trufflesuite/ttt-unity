using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject newGameSetup;

    // Main Menu

    public void GoToNewGame()
    {
        mainMenu.gameObject.SetActive(false);
        newGameSetup.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    // New Game Setup

    public void StartGame()
    {
        SceneManager.LoadScene("TicTacToe");
    }

    public void BackToMainMenuFromNewGameSetup()
    {
        newGameSetup.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }
}
