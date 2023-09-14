using deVoid.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SpaceInvaders
{
    public class SceneManager : MonoBehaviour
    {
        public GameObject gameUI;     // The game UI
        public GameInput gameInput;

        private bool gameStarted = false;
        private bool gamePaused = false;
        int score = 0;

        private void Awake()
        {
            Signals.Get<Project.MainMenu.QuitSignal>().AddListener(OnQuit);
            Signals.Get<Project.MainMenu.PlaySignal>().AddListener(OnPlayOrResume);
            Signals.Get<Project.SceneManager.HighScoreSignal>().AddListener(OnHighScore);
            Signals.Get<Project.SceneManager.MainMenuSignal>().AddListener(OnShowMainMenu);
            Signals.Get<Project.Game.NoMoreLivesSignal>().AddListener(OnQuit);
            Signals.Get<Project.Game.AlienKilledSignal>().AddListener(OnAlienKilled);
            Signals.Get<Project.Game.UFOKilledSignal>().AddListener(OnUFOKilled);
            gameInput.OnPauseAction += OnGameInputPause;
        }
        private void OnDestroy()
        {
            Signals.Get<Project.MainMenu.QuitSignal>().RemoveListener(OnQuit);
            Signals.Get<Project.MainMenu.PlaySignal>().RemoveListener(OnPlayOrResume);
            Signals.Get<Project.SceneManager.HighScoreSignal>().RemoveListener(OnHighScore);
            Signals.Get<Project.SceneManager.MainMenuSignal>().RemoveListener(OnShowMainMenu);
            Signals.Get<Project.Game.NoMoreLivesSignal>().RemoveListener(OnQuit);
            Signals.Get<Project.Game.AlienKilledSignal>().RemoveListener(OnAlienKilled);
            Signals.Get<Project.Game.UFOKilledSignal>().RemoveListener(OnUFOKilled);
            gameInput.OnPauseAction -= OnGameInputPause;
        }

        private void OnUFOKilled(UFOAlien ufo)
        {
            score += ufo.points;
            Signals.Get<Project.Game.ScoreUpdatedSignal>().Dispatch(score);
        }
        private void OnAlienKilled(Alien alien)
        {
            score += alien.points;
            Signals.Get<Project.Game.ScoreUpdatedSignal>().Dispatch(score);
        }

        //  The GameInputOnInteractAction event is called from the GameInput when the user presses space
        private void OnGameInputPause(object sender, System.EventArgs e)
        {
            TogglePauseGame();
        }

        private void Start()
        {
            // Initial setup
            OnShowMainMenu();
            //HideGameElements();
        }

        private void OnShowMainMenu()
        {

            if (!gameStarted)
            {
                gameUI.SetActive(false);
            }
        }

        private void HideMainMenu()
        {
            EventSystem.current.SetSelectedGameObject(null);
            gameUI.SetActive(true);
        }
        public void OnPlayOrResume()
        {
            if (!gameStarted)
            {
                gameStarted = true;
                // Start the game
                HideMainMenu();
                //ShowGameElements();
                Signals.Get<Project.SceneManager.LoadGameSignal>().Dispatch();

            }
            else if (gamePaused)
            {
                // Resume game
                gamePaused = false;
                HideMainMenu();
                //ShowGameElements();
                // Unpause game logic here
            }
            Time.timeScale = 1;
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
                ObjectPooler.Instance.ImmediateReturnAllDelayedObjects();
                Signals.Get<Project.SceneManager.ResetGameSignal>().Dispatch();
                Time.timeScale = 1;
                score = 0;
            }
        }
        public void TogglePauseGame()
        {
            if (gameStarted)
            {
                if (gamePaused)
                {
                    OnPlayOrResume();
                }
                else
                {
                    Time.timeScale = 0;
                    gamePaused = true;
                }
                Signals.Get<Project.SceneManager.GamePausedSignal>().Dispatch(gamePaused);
            }
        }
    }
}