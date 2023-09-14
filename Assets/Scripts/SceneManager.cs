using deVoid.Utils;
using UnityEngine;


namespace SpaceInvaders
{
    public class SceneManager : MonoBehaviour
    {
        public GameObject mainMenuUI; // The main menu UI
        public GameObject gameUI;     // The game UI
        public GameObject gameElements; // The game elements like player, enemies, etc.

        private bool gameStarted = false;
        private bool gamePaused = false;

        private void Awake()
        {
            Signals.Get<Project.SceneManager.PlaySignal>().AddListener(OnPlayOrResume);
            Signals.Get<Project.SceneManager.HighScoreSignal>().AddListener(OnHighScore);
            Signals.Get<Project.SceneManager.MainMenuSignal>().AddListener(ShowMainMenu);
        }
        private void OnDestroy()
        {
            Signals.Get<Project.SceneManager.PlaySignal>().RemoveListener(OnPlayOrResume);
            Signals.Get<Project.SceneManager.HighScoreSignal>().RemoveListener(OnHighScore);
            Signals.Get<Project.SceneManager.MainMenuSignal>().RemoveListener(ShowMainMenu);
        }
        private void Start()
        {
            // Initial setup
            ShowMainMenu();
            HideGameElements();
        }

        private void ShowMainMenu()
        {
            mainMenuUI.SetActive(true);
            gameUI.SetActive(false);
        }

        private void HideMainMenu()
        {
            mainMenuUI.SetActive(false);
            gameUI.SetActive(true);
        }

        private void ShowGameElements()
        {
            gameElements.SetActive(true);
        }

        private void HideGameElements()
        {
            gameElements.SetActive(false);
        }

        public void OnPlayOrResume()
        {
            if (!gameStarted)
            {
                gameStarted = true;
                // Start the game
                HideMainMenu();
                ShowGameElements();
            }
            else if (gamePaused)
            {
                // Resume game
                gamePaused = false;
                HideMainMenu();
                ShowGameElements();
                // Unpause game logic here
            }
        }

        public void OnHighScore()
        {
            // Show high scores
        }

        public void OnQuit()
        {
            // If in the middle of a game, stop it
            if (gameStarted)
            {
                gameStarted = false;
                HideGameElements();
                ShowMainMenu();
            }
            else
            {
                // If in main menu, then just close the app
                Application.Quit();
            }
        }

        public void OnEscape()
        {
            if (gameStarted)
            {
                if (gamePaused)
                {
                    OnPlayOrResume();
                }
                else
                {
                    gamePaused = true;
                    ShowMainMenu();
                    HideGameElements();
                    // Pause game logic here
                }
            }
        }
    }
}