using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum EGameState
{
    menu,
    gameplay,
    gameover
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject connectingToServer, mainMenu, gameOver;
    public PhotonManager photonManager;
    public Text textGameOver;

    PlayerControls controls;
    EGameState state;

    void Awake()
    {
        if (instance != null) Destroy(this);
        else instance = this;

        controls = new PlayerControls();

        controls.Menu.AnyButton.performed += ctx => AnyButton();
    }

    public void MenuMode()
    {
        Time.timeScale = 1;
        state = EGameState.menu;
        controls.Menu.Disable();
        Invoke("ActivateMenuControls", 1);
        controls.Menu.Enable();
        connectingToServer.SetActive(false);
        mainMenu.SetActive(true);
        gameOver.SetActive(false);

        photonManager.LeaveRoom();
    }

    void ActivateMenuControls()
    {
        controls.Menu.Enable();
    }

    void GameplayMode()
    {
        if (photonManager.CanCreateRoom())
        {
            Time.timeScale = 1;
            state = EGameState.gameplay;
            controls.Menu.Disable();
            connectingToServer.SetActive(false);
            mainMenu.SetActive(false);
            gameOver.SetActive(false);

            photonManager.JoinRoom();
        }
    }

    public void GameOverMode(bool win)
    {
        Time.timeScale = 0f;
        state = EGameState.gameover;
        controls.Menu.Enable();
        connectingToServer.SetActive(false);
        mainMenu.SetActive(false);
        gameOver.SetActive(true);

        if (win) textGameOver.text = "YOU WIN!";
        else textGameOver.text = "You Lose";
    }

    void AnyButton()
    {
        if (state == EGameState.menu)
        {
            GameplayMode();
        }
        else if (state == EGameState.gameover)
        {
            MenuMode();
        }
    }
}
