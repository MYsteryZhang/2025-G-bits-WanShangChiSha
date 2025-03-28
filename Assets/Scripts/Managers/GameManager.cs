using System;
using UnityEngine;
using static DialogueConfig;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    private bool isGamePaused = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateGameState(GameState.Start);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Start:
                HandleGameStart();
                break;
            case GameState.Pause:
                HandlePause();
                break;
            case GameState.InGame:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandlePause()
    {
        PauseGame();
    }

    public void PauseGame()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            Time.timeScale = isGamePaused ? 0 : 1; // ÔÝÍ£ÓÎÏ·Âß¼­  
            AudioListener.pause = isGamePaused;     // ÔÝÍ£ÉùÒô  
            Cursor.lockState = CursorLockMode.Confined;
            UIManager.Instance.TogglePauseMenu(isGamePaused);
        }
        else
        {
            isGamePaused = false;
            Time.timeScale = isGamePaused ? 0 : 1;
            AudioListener.pause = isGamePaused;
            UIManager.Instance.TogglePauseMenu(false);
            Cursor.lockState = CursorLockMode.Locked;
            UpdateGameState(GameState.InGame);
        }
    }

    private void HandleGameStart()
    {
        DialogManager.Instance.StartDialogue("GameStart_ControlPrompt", 5f);
    }

    public enum GameState
    {
        Start,
        Pause,
        InGame
    }
}
