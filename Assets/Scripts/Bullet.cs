using UnityEngine;

namespace SpaceInvaders
{
    public abstract class Bullet : MonoBehaviour
    {
        protected float speed = 5.0f;
        protected Vector3 direction;

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            transform.position += direction * (speed) * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleCollision(other);
        }
        protected abstract void HandleCollision(Collider other);
    }
}
