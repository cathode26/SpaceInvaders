using deVoid.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders
{
    public class HighScores : MonoBehaviour
    {
        [SerializeField]
        private Button backButton;
        [SerializeField]
        private GameObject ui;
        private void Awake()
        {
            ui = transform.GetChild(0).gameObject;

            Signals.Get<Project.MainMenu.OnHighScoresPressedSignal>().AddListener(OnShowHighScores);

            backButton.onClick.AddListener(() =>
            {
                Signals.Get<Project.HighScores.OnBackPressedSignal>().Dispatch();
                ui.SetActive(false);
            });
        }
        private void OnDestroy()
        {
            Signals.Get<Project.MainMenu.OnHighScoresPressedSignal>().RemoveListener(OnShowHighScores);
        }
        private void OnShowHighScores()
        {
            ui.SetActive(true);
        }
    }
}
