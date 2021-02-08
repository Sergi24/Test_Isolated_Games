using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EGameState
{
    menu,
    gameplay,
    gameover
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject mainMenu, gameOver;
    public PhotonManager photonManager;

    PlayerControls controls;
    private EGameState state;

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
        controls.Menu.Enable();
        mainMenu.SetActive(true);
        gameOver.SetActive(false);

        photonManager.LeaveRoom();
    }

    void GameplayMode()
    {
        if (photonManager.CanCreateRoom())
        {
            Time.timeScale = 1;
            state = EGameState.gameplay;
            controls.Menu.Disable();
            mainMenu.SetActive(false);
            gameOver.SetActive(false);

            photonManager.JoinRoom();
        }
    }

    public void GameOverMode()
    {
        Time.timeScale = 0f;
        state = EGameState.gameover;
        controls.Menu.Enable();
        mainMenu.SetActive(false);
        gameOver.SetActive(true);
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
