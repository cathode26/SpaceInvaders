using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.00001f;

    private Vector3 direction;

    private void Start()
    {
        direction = Vector3.up; // The bullet travels upwards
    }

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
        // If collided with an Alien
        if (other.gameObject.GetComponent<Alien>())
        {
            // The Alien script will handle its own destruction and explosion
            // We just need to destroy the bullet
            Destroy(gameObject);
        }
    }
}
