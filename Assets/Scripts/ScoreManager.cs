using deVoid.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpaceInvaders
{
    public class ScoreManager : MonoBehaviour
    {
        private const string HIGH_SCORE_KEY = "SpaceInvadersHighScore";
        public int SavedRank { get; private set; } = 0;
        public string SavedName { get; private set; } = "";
        public int CurrentScore { get; private set; } = 0;
        public int HighScore { get; private set; } = 0;
        public int LowestHighScore { get; private set; } = 0;
        public IEnumerable<(string name, int score)> HighScores
        {
            get { return _highScores.AsReadOnly(); }
        }
        private List<(string name, int score)> _highScores = new List<(string name, int score)>();

        private void Awake()
        {
            LoadHighScores();
            Signals.Get<Project.SceneManager.LoadGameSignal>().AddListener(OnLoadGame);
            Signals.Get<Project.Game.AlienKilledSignal>().AddListener(OnAlienKilled);
            Signals.Get<Project.Game.UFOKilledSignal>().AddListener(OnUFOKilled);
            Signals.Get<Project.SceneManager.OnResetGameCompleteSignal>().AddListener(OnResetGameComplete);
        }
        private void OnDestroy()
        {
            Signals.Get<Project.SceneManager.LoadGameSignal>().RemoveListener(OnLoadGame);
            Signals.Get<Project.Game.AlienKilledSignal>().RemoveListener(OnAlienKilled);
            Signals.Get<Project.Game.UFOKilledSignal>().RemoveListener(OnUFOKilled);
            Signals.Get<Project.SceneManager.OnResetGameCompleteSignal>().RemoveListener(OnResetGameComplete);
            SaveHighScores();
        }
        private void OnLoadGame()
        {
            ResetCurrentScore();
            Signals.Get<Project.Game.HighScoreUpdatedSignal>().Dispatch(HighScore);
            Signals.Get<Project.Game.ScoreUpdatedSignal>().Dispatch(CurrentScore);
        }
        private void OnResetGameComplete()
        {
            SaveHighScores();
            if (CurrentScore > 0 && (CurrentScore > LowestHighScore || _highScores.Count < 10))
                Signals.Get<Project.MainMenu.OnHighScoresPressedSignal>().Dispatch();
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
        private void AddScore(int points)
        {
            CurrentScore += points;
            if (CurrentScore > HighScore)
            {
                HighScore = CurrentScore;
                Signals.Get<Project.Game.HighScoreUpdatedSignal>().Dispatch(HighScore);
            }
            
            if (CurrentScore > LowestHighScore || _highScores.Count < 10)
            {
                if (SavedRank == 0)
                    InsertScoreCalculateRank();
                else
                    UpdateScoreUpdateRank();
            }
        }
        private void ResetCurrentScore()
        {
            CurrentScore = 0;
            SavedRank = 0;
            SavedName = "";
        }
        private void SaveHighScores()
        {
            string serializedScores = "";
            foreach (var score in HighScores)
            {
                serializedScores += score.name + "," + score.score.ToString() + ";";
            }

            PlayerPrefs.SetString(HIGH_SCORE_KEY, serializedScores);
            PlayerPrefs.Save();
        }
        private void LoadHighScores()
        {
            if (PlayerPrefs.HasKey(HIGH_SCORE_KEY))
            {
                _highScores.Clear();
                string serializedScores = PlayerPrefs.GetString(HIGH_SCORE_KEY);
                string[] scorePairs = serializedScores.Split(';');

                foreach (var pair in scorePairs)
                {
                    if (!string.IsNullOrEmpty(pair))
                    {
                        string[] data = pair.Split(',');
                        string playerName = data[0];
                        int playerScore = int.Parse(data[1]);

                        _highScores.Add((playerName, playerScore));
                    }
                }

                if (_highScores.Count > 0)
                {
                    LowestHighScore = _highScores[_highScores.Count - 1].score;
                    HighScore = _highScores[0].score;
                }
            }
        }
        private int GetInsertionIndex(int score)
        {
            return HighScores.Count(s => s.score > score);
        }
        private void InsertScoreCalculateRank()
        {
            int index = GetInsertionIndex(CurrentScore);
            // Insert new score and remove the last one
            _highScores.Insert(index, ("", CurrentScore));
            UpdateScoreData(index);
        }
        private void UpdateScoreUpdateRank()
        {
            int index = GetInsertionIndex(CurrentScore);
            if (index != HighScore - 1)
            {
                (string name, int score) curScore = (_highScores[SavedRank - 1].name, CurrentScore);
                _highScores.RemoveAt(SavedRank - 1);
                _highScores.Insert(index, curScore);
            }
            else
            {
                _highScores[index] = (_highScores[index].name, CurrentScore);
            }
            UpdateScoreData(index);
        }
        private void UpdateScoreData(int index)
        {
            if (_highScores.Count > 10)
                _highScores.RemoveAt(_highScores.Count - 1);

            SavedRank = index + 1;
            LowestHighScore = _highScores[_highScores.Count - 1].score;
            HighScore = _highScores[0].score;
        }
        public bool NewHighScore()
        {
            if (SavedRank > 0)
                return true;
            else
                return false;
        }
        public void SetPlayerName(string playerName)
        {
            SavedName = playerName;
            _highScores[SavedRank - 1] = (SavedName, _highScores[SavedRank - 1].score);
            SaveHighScores();
        }
    }
}
