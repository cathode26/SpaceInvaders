using deVoid.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpaceInvaders
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject ui;
        [SerializeField]
        private Button playButton;
        [SerializeField]
        private Button highScoreButton;
        [SerializeField]
        private Button quitButton;

        private void Awake()
        {
            ui = transform.GetChild(0).gameObject;

            Signals.Get<Project.HighScores.OnBackPressedSignal>().AddListener(OnHighScoresBackPressed);
            Signals.Get<Project.SceneManager.GamePausedSignal>().AddListener(OnGamePaused);
            Signals.Get<Project.SceneManager.ResetGameSignal>().AddListener(OnResetGame);

            playButton.onClick.AddListener(() =>
            {
                TextMeshProUGUI buttonText = playButton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = "CONTINUE";
                Signals.Get<Project.MainMenu.PlaySignal>().Dispatch();
                quitButton.interactable = true;
                ui.SetActive(false);
            });
            highScoreButton.onClick.AddListener(() =>
            {
                Signals.Get<Project.MainMenu.OnHighScoresPressedSignal>().Dispatch();
                ui.SetActive(false);
            });
            quitButton.onClick.AddListener(() =>
            {
                TextMeshProUGUI buttonText = playButton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = "PLAY";
                quitButton.interactable = false;
                Signals.Get<Project.MainMenu.QuitSignal>().Dispatch();
            });
        }
        private void OnDestroy()
        {
            Signals.Get<Project.HighScores.OnBackPressedSignal>().RemoveListener(OnHighScoresBackPressed);
            Signals.Get<Project.SceneManager.GamePausedSignal>().RemoveListener(OnGamePaused);
            Signals.Get<Project.SceneManager.ResetGameSignal>().RemoveListener(OnResetGame);
        }
        private void OnHighScoresBackPressed()
        {
            ui.SetActive(true);
        }
        private void OnGamePaused(bool paused)
        {
            if (paused)
                ui.SetActive(true);
            else
                ui.SetActive(false);
        }
        private void OnResetGame()
        {
            TextMeshProUGUI buttonText = playButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = "PLAY";
            quitButton.interactable = false;
            ui.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null); // Clear selected UI element
        }
    }
}