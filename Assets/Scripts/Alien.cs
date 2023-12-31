using deVoid.Utils;
using UnityEngine;
using static SpaceInvaders.PrefabTypes;

namespace SpaceInvaders
{
    public class Alien : MonoBehaviour
    {
        [Header("Explosion Settings")]
        public GameObject explosionPrefab; // Reference to the voxel explosion prefab
        public float explosionDuration = 0.5f; // Duration the explosion lasts
        public float shootingInterval = 1.0f;
        public int points = 5;

        [SerializeField]
        private SpawnableType spawnableType;
        private bool isDying = false;
        private bool isAlive = false;
        public bool IsAlive { get => isAlive; private set => isAlive = value; }
        public SpawnableType SpawnableType { get => spawnableType; private set => spawnableType = value; }

        public void OnEnable()
        {
            isAlive = true;
            isDying = false;
        }
        public void OnDisable()
        {
            isAlive = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            // If collided with a bullet and not already in the dying process
            if (other.gameObject.GetComponent<PlayerBullet>() && !isDying)
            {
                isDying = true;
                HandleDeath();
            }
        }
        private void HandleDeath()
        {
            // Disable the alien (make it invisible)
            gameObject.SetActive(false);

            // Instantiate the explosion at the alien's position
            GameObject explosion = ObjectPooler.Instance.RequestObject(SpawnableType.Explosion, transform.position, Quaternion.identity);
            if (explosion != null)
            {
                // Apply a tiny random offset to simulate explosion movement
                explosion.transform.position += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));

                // Return the explosion after a brief duration
                ObjectPooler.Instance.ReturnObject(SpawnableType.Explosion, explosion, explosionDuration);
            }

            // Return yourself the alien after the explosion ends
            ObjectPooler.Instance.ReturnObject(spawnableType, gameObject);
            Signals.Get<Project.Game.AlienKilledSignal>().Dispatch(this);
        }
        public void Shoot()
        {
            Vector3 spawnPosition = transform.position;
            // Request a Enemy Bullet from ObjectPooler 
            ObjectPooler.Instance.RequestObject(SpawnableType.EnemyBullet, spawnPosition, Quaternion.identity);
        }
        public void Kill()
        {
            // Disable the alien (make it invisible)
            gameObject.SetActive(false);
            // Return yourself the alien after the explosion ends
            ObjectPooler.Instance.ReturnObject(spawnableType, gameObject);
        }
    }
}