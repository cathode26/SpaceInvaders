using UnityEngine;
using static SpaceInvaders.PrefabTypes;

namespace SpaceInvaders
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float speed = 0.00001f;
        [SerializeField]
        private float topBound = 10f; // The y-value beyond which the bullet should be returned to the pool

        private Vector3 direction;

        private void Start()
        {
            direction = Vector3.up; // The bullet travels upwards
        }

        private void Update()
        {
            Move();

            // If the bullet goes off the top of the screen, deactivate it
            if (transform.position.y > topBound)
            {
                ReturnToPool();
            }
        }

        private void Move()
        {
            transform.position += direction * (speed) * Time.deltaTime;
        }
        private void OnTriggerEnter(Collider other)
        {
            // If collided with an Alien
            if (other.gameObject.GetComponent<Alien>())
            {
                // The Alien script will handle its own destruction and explosion
                ReturnToPool();
            }
        }
        private void ReturnToPool()
        {
            ObjectPooler.Instance.ReturnObject(SpawnableType.Bullet, gameObject);
        }
    }
}