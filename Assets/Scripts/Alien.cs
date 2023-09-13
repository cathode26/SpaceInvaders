using UnityEngine;
using static SpaceInvaders.PrefabTypes;

namespace SpaceInvaders
{
    public class Alien : MonoBehaviour
    {
        [Header("Explosion Settings")]
        public GameObject explosionPrefab; // Reference to the voxel explosion prefab
        public float explosionDuration = 0.5f; // Duration the explosion lasts

        [SerializeField]
        private SpawnableType spawnableType;
        private bool isDying = false;
        private bool isAlive = true;
        public bool IsAlive { get => isAlive; private set => isAlive = value; }

        private void OnTriggerEnter(Collider other)
        {
            // If collided with a bullet and not already in the dying process
            if (other.gameObject.GetComponent<Bullet>() && !isDying)
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
                ObjectPooler.Instance.ReturnObject(spawnableType, gameObject, explosionDuration);
            }

            // Return yourself the alien after the explosion ends
            ObjectPooler.Instance.ReturnObject(spawnableType, gameObject);
        }
    }
}