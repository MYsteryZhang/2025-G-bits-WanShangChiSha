using System;
using UnityEngine;
using static DialogueConfig;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;
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
                break;
            case GameState.InGame:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
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
