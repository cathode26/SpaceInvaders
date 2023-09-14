using deVoid.Utils;
using UnityEngine;
using static SpaceInvaders.PrefabTypes;

namespace SpaceInvaders
{
    public class EnemyBullet : Bullet
    {
        private void Awake()
        {
            direction = Vector3.down;
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
            ObjectPooler.Instance.ReturnObject(SpawnableType.EnemyBullet, gameObject);
        }
        protected override void HandleCollision(Collider other)
        {
            if (other.GetComponent<Player>() || other.GetComponent<Boundary>())
            {
                // Handle collision with the player
                // For example, decrease player health or trigger a game over

                // Return bullet to the pool
                ObjectPooler.Instance.ReturnObject(SpawnableType.EnemyBullet, gameObject);
            }
        }
    }
}