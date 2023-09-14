using deVoid.Utils;
using UnityEngine;

namespace SpaceInvaders
{
    public class ScoreManager : MonoBehaviour
    {
        private const string HIGH_SCORE_KEY = "SpaceInvadersHighScore";

        public int CurrentScore { get; private set; } = 0;
        public int HighScore { get; private set; } = 0;

        private void Awake()
        {
            LoadHighScore();
            Signals.Get<Project.SceneManager.LoadGameSignal>().AddListener(OnLoadGame);
            Signals.Get<Project.Game.AlienKilledSignal>().AddListener(OnAlienKilled);
            Signals.Get<Project.Game.UFOKilledSignal>().AddListener(OnUFOKilled);
        }
        private void OnDestroy()
        {
            Signals.Get<Project.SceneManager.LoadGameSignal>().RemoveListener(OnLoadGame);
            Signals.Get<Project.Game.AlienKilledSignal>().RemoveListener(OnAlienKilled);
            Signals.Get<Project.Game.UFOKilledSignal>().RemoveListener(OnUFOKilled);
            SaveHighScore();
        }
        private void OnLoadGame()
        {
            ResetCurrentScore();
            Signals.Get<Project.Game.HighScoreUpdatedSignal>().Dispatch(HighScore);
            Signals.Get<Project.Game.ScoreUpdatedSignal>().Dispatch(CurrentScore);
        }
        private void OnUFOKilled(UFOAlien ufo)
        {
            AddScore(ufo.points);
            Signals.Get<Project.Game.ScoreUpdatedSignal>().Dispatch(CurrentScore);
        }
        private void OnAlienKilled(Alien alien)
        {
            AddScore(alien.points);
            Signals.Get<Project.Game.ScoreUpdatedSignal>().Dispatch(CurrentScore);
        }

        public void AddScore(int points)
        {
            CurrentScore += points;
            if (CurrentScore > HighScore)
            {
                HighScore = CurrentScore;
                Signals.Get<Project.Game.HighScoreUpdatedSignal>().Dispatch(HighScore);
                SaveHighScore();
            }
        }
        public void ResetCurrentScore()
        {
            CurrentScore = 0;
        }
        private void SaveHighScore()
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, HighScore);
            PlayerPrefs.Save();
        }
        private void LoadHighScore()
        {
            HighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        }
    }
}
