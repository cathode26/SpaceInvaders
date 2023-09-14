using deVoid.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private Button playButton;
        [SerializeField]
        private Button highScoreButton;
        [SerializeField]
        private Button quitButton;

        private void Awake()
        {
            playButton.onClick.AddListener(() =>
            {
                Signals.Get<Project.SceneManager.PlaySignal>().Dispatch();
            });
            highScoreButton.onClick.AddListener(() =>
            {
                Signals.Get<Project.SceneManager.HighScoreSignal>().Dispatch();
            });
            quitButton.onClick.AddListener(() =>
            {
                Signals.Get<Project.SceneManager.QuitSignal>().Dispatch();
            });
            Time.timeScale = 1.0f;
        }
    }
}