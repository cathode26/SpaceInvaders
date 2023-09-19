using deVoid.Utils;
using UnityEngine;

namespace SpaceInvaders
{
    public class SceneManager : MonoBehaviour
    {
        public GameInput gameInput;

        private bool gameStarted = false;
        private bool gamePaused = false;

        private void Awake()
        {
            Signals.Get<Project.MainMenu.QuitSignal>().AddListener(OnQuit);
            Signals.Get<Project.MainMenu.PlaySignal>().AddListener(OnPlayOrResume);
            Signals.Get<Project.Game.NoMoreLivesSignal>().AddListener(OnQuit);
            
            gameInput.OnPauseAction += OnGameInputPause;
        }
        private void OnDestroy()
        {
            Signals.Get<Project.MainMenu.QuitSignal>().RemoveListener(OnQuit);
            Signals.Get<Project.MainMenu.PlaySignal>().RemoveListener(OnPlayOrResume);
            Signals.Get<Project.Game.NoMoreLivesSignal>().RemoveListener(OnQuit);
            
            gameInput.OnPauseAction -= OnGameInputPause;
        }

        //  The GameInputOnInteractAction event is called from the GameInput when the user presses space
        private void OnGameInputPause(object sender, System.EventArgs e)
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

        public void OnPlayOrResume()
        {
            if (!gameStarted)
            {
                gameStarted = true;
                // Start the game
                Signals.Get<Project.SceneManager.LoadGameSignal>().Dispatch();

            }
            else if (gamePaused)
            {
                // Resume game
                gamePaused = false;
                Signals.Get<Project.SceneManager.GamePausedSignal>().Dispatch(gamePaused);
            }
            Time.timeScale = 1;
        }

        public void OnQuit()
        {
            // If in the middle of a game, stop it
            if (gameStarted)
            {
                gameStarted = false;
                ObjectPooler.Instance.ImmediateReturnAllDelayedObjects();
                Signals.Get<Project.SceneManager.ResetGameSignal>().Dispatch();
                Signals.Get<Project.SceneManager.OnResetGameCompleteSignal>().Dispatch();
                Time.timeScale = 1;
            }
        }
    }
}