using UnityEngine;

public class Alien : MonoBehaviour
{
    [Header("Explosion Settings")]
    public GameObject explosionPrefab; // Reference to the voxel explosion prefab
    public float explosionDuration = 0.5f; // Duration the explosion lasts

    private bool isDying = false;

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
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Apply a tiny random offset to simulate explosion movement
        explosion.transform.position += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));

        // Destroy the explosion after a brief duration
        Destroy(explosion, explosionDuration);

        // Destroy the alien after the explosion ends
        Destroy(gameObject, explosionDuration);
    }
}
