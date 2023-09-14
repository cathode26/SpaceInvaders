using deVoid.Utils;
using UnityEngine;

namespace SpaceInvaders
{
    public class MainMenuElements : MonoBehaviour
    {
        [SerializeField]
        private GameObject menuElements;
        private void Awake()
        {
            Signals.Get<Project.SceneManager.LoadGameSignal>().AddListener(OnLoadGame);
            Signals.Get<Project.SceneManager.ResetGameSignal>().AddListener(OnResetGame);

        }
        private void OnDestroy()
        {
            Signals.Get<Project.SceneManager.LoadGameSignal>().RemoveListener(OnLoadGame);
            Signals.Get<Project.SceneManager.ResetGameSignal>().RemoveListener(OnResetGame);
        }

        private void OnResetGame()
        {
            menuElements.SetActive(true);
        }
        private void OnLoadGame()
        {
            menuElements.SetActive(false);
        }
    }
}
