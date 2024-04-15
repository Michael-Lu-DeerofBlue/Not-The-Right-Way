using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public enum GameState { MainMenu, InGame }
    private GameState currentState;

    private void Start()
    {
        SetGameState(GameState.MainMenu);
    }

    public void SetGameState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.MainMenu:
                BGMManager.Instance.PlayMainMenuMusic();
                break;
            case GameState.InGame:
                BGMManager.Instance.PlayInGameMusic();
                break;
        }
    }

    // Call this method when player starts the game
    public void StartGame()
    {
        SetGameState(GameState.InGame);
    }

    // Call this method to return to the main menu
    public void GoToMainMenu()
    {
        SetGameState(GameState.MainMenu);
    }
}
