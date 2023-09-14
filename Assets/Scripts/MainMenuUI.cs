using deVoid.Utils;
using TMPro;
using UnityEngine;
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

            playButton.onClick.AddListener(() =>
            {
                TextMeshProUGUI buttonText = playButton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = "CONTINUE";
                Signals.Get<Project.SceneManager.PlaySignal>().Dispatch();
                quitButton.interactable = true;
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
                Signals.Get<Project.SceneManager.QuitSignal>().Dispatch();
            });
        }
        private void OnDestroy()
        {
            Signals.Get<Project.HighScores.OnBackPressedSignal>().RemoveListener(OnHighScoresBackPressed);
        }
        private void OnHighScoresBackPressed()
        {
            ui.SetActive(true);
        }

    }
}