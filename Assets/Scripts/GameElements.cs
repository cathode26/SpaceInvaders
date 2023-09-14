using deVoid.Utils;
using UnityEngine;

namespace SpaceInvaders
{
    public class GameElements : MonoBehaviour
    {
        [SerializeField]
        private GameObject gameElements;
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
            Signals.Get<Project.Game.ResetGameSignal>().Dispatch();
            gameElements.SetActive(false);
        }
        private void OnLoadGame()
        {
            gameElements.SetActive(true);
            Signals.Get<Project.Game.LoadGameSignal>().Dispatch();
        }
    }
}
