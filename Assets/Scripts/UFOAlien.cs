using deVoid.Utils;
using UnityEngine;
using static SpaceInvaders.PrefabTypes;

namespace SpaceInvaders
{
    public class UFOAlien : MonoBehaviour
    {
        public float speed = 1f;
        public int points = 200;

        private bool isMovingRight;
        private bool isDying = false;
        public float explosionDuration = 0.5f; // Duration the explosion lasts

        private void OnEnable()
        {
            isDying = false;
        }
        void Update()
        {
            if (isMovingRight)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }

            // Check if UFO has moved off-screen, and if so, deactivate it
            if ((isMovingRight && transform.position.x > 10) || (!isMovingRight && transform.position.x < -10))
            {
                ObjectPooler.Instance.ReturnObject(PrefabTypes.SpawnableType.UFOAlien, gameObject);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            // If collided with a bullet and not already in the dying process
            if (other.gameObject.GetComponent<PlayerBullet>() && !isDying)
            {
                isDying = true;
                ShotDown();
            }
        }
        public void SetDirection(bool isMovingRight)
        {
            this.isMovingRight = isMovingRight;
        }
        public void ShotDown()
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
            ObjectPooler.Instance.ReturnObject(PrefabTypes.SpawnableType.UFOAlien, gameObject);

            // ScoreManager to manage the player's score:
            Signals.Get<Project.Game.UFOKilledSignal>().Dispatch(this);
        }
    }
}