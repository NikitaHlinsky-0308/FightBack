using System;
using CodeBase.Player;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public enum GameState
    {
        Menu,
        GameLoop,
        LevelComplete,
        Gameover,
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private GameState gameState;

        public static Action<GameState> onGameStateChanged;

        private void Awake()
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
        }

        public void SetGameState(GameState gameState)
        {
            this.gameState = gameState;
            onGameStateChanged?.Invoke(gameState);

            Debug.Log("Game State changed to : " + gameState);
        }
    }
}