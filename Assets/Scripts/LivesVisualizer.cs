using deVoid.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class LivesVisualizer : MonoBehaviour
    {
        public List<GameObject> lifeDummies;

        private void OnEnable()
        {
            Signals.Get<Project.Game.LivesChangedSignal>().AddListener(OnLivesChanged);
            Signals.Get<Project.SceneManager.ResetGameSignal>().AddListener(OnResetGame);
        }
        private void OnDisable()
        {
            Signals.Get<Project.Game.LivesChangedSignal>().RemoveListener(OnLivesChanged);
            Signals.Get<Project.SceneManager.ResetGameSignal>().RemoveListener(OnResetGame);
        }

        private void Start()
        {
            // Ensure all dummies are initially active
            foreach (GameObject dummy in lifeDummies)
            {
                dummy.SetActive(true);
            }
        }
        public void OnLivesChanged(int lives)
        {
            lives--;
            if (lives >= 0) 
            {
                lifeDummies[lives].SetActive(false); // Deactivate the last active dummy
            }
        }
        public void OnResetGame()
        {
            // Ensure all dummies are initially active
            foreach (GameObject dummy in lifeDummies)
            {
                dummy.SetActive(true);
            }
        }
    }
}