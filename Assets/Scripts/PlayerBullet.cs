using deVoid.Utils;
using UnityEngine;
using static SpaceInvaders.PrefabTypes;

namespace SpaceInvaders
{
    public class PlayerBullet : Bullet
    {
        private void Awake()
        {
            direction = Vector3.up;
        }
        private void OnEnable()
        {
            Signals.Get<Project.SceneManager.ResetGameSignal>().AddListener(OnResetGame);
        }
        private void OnDisable()
        {
            Signals.Get<Project.SceneManager.ResetGameSignal>().RemoveListener(OnResetGame);
        }
        private void OnResetGame()
        {
            //if we are enabled during a reset it means we need to be returned
            ObjectPooler.Instance.ReturnObject(PrefabTypes.SpawnableType.PlayerBullet, gameObject);
        }
        protected override void HandleCollision(Collider other)
        {
            if (other.GetComponent<Alien>() || other.GetComponent<Boundary>())
            {
                // Handle collision with enemy
                // For example, destroy the enemy or decrease its health

                // Return bullet to the pool
                ObjectPooler.Instance.ReturnObject(SpawnableType.PlayerBullet, gameObject);
            }
        }
    }
}