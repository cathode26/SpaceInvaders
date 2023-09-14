using deVoid.Utils;
using TMPro;
using UnityEngine;

namespace SpaceInvaders
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI highScoreText;
        [SerializeField]
        private TextMeshProUGUI scoreText;
        [SerializeField]
        private TextMeshProUGUI livesText;
        [SerializeField]
        GameObject ui;

        private int highScore;
        private int score;
        private int lives = 3;

        private void OnEnable()
        {
            Signals.Get<Project.SceneManager.GamePausedSignal>().AddListener(OnGamePaused);
            Signals.Get<Project.Game.ScoreUpdatedSignal>().AddListener(OnScoreUpdated);
            Signals.Get<Project.Game.HighScoreUpdatedSignal>().AddListener(OnHighScoreUpdated);
            Signals.Get<Project.Game.LivesChangedSignal>().AddListener(OnLivesChanged);
            Signals.Get<Project.SceneManager.LoadGameSignal>().AddListener(OnLoadGame);
            Signals.Get<Project.SceneManager.ResetGameSignal>().AddListener(OnResetGame);
        }
        private void OnDisable()
        {
            Signals.Get<Project.SceneManager.GamePausedSignal>().RemoveListener(OnGamePaused);
            Signals.Get<Project.Game.ScoreUpdatedSignal>().RemoveListener(OnScoreUpdated);
            Signals.Get<Project.Game.HighScoreUpdatedSignal>().RemoveListener(OnHighScoreUpdated);
            Signals.Get<Project.Game.LivesChangedSignal>().RemoveListener(OnLivesChanged);
            Signals.Get<Project.SceneManager.LoadGameSignal>().RemoveListener(OnLoadGame);
            Signals.Get<Project.SceneManager.ResetGameSignal>().RemoveListener(OnResetGame);
        }
        private void OnGamePaused(bool paused)
        {
            if(paused == true)
                ui.SetActive(false);
            else
                ui.SetActive(true);
        }
        private void OnLoadGame()
        {
            ui.SetActive(true);
        }
        private void OnResetGame()
        {
            OnScoreUpdated(0);
            OnLivesChanged(3);
            ui.SetActive(false);
        }
        private void OnScoreUpdated(int newScore)
        {
            score = newScore;
            scoreText.text = score.ToString();
        }
        private void OnHighScoreUpdated(int newHighScore)
        {
            highScore = newHighScore;
            highScoreText.text = highScore.ToString();
        }
        private void OnLivesChanged(int newLives)
        {
            lives = newLives;
            livesText.text = lives.ToString();
        }
    }
}
