using SpaceInvaders;
using UnityEngine;
using static SpaceInvaders.PrefabTypes;

public class PlayerBullet : Bullet
{
    private void Awake()
    {
        direction = Vector3.up;
    }

    protected override void HandleCollision(Collider other)
    {
        if (other.GetComponent <Alien>() || other.GetComponent<Boundary>())
        {
            // Handle collision with enemy
            // For example, destroy the enemy or decrease its health

            // Return bullet to the pool
            ObjectPooler.Instance.ReturnObject(SpawnableType.PlayerBullet, gameObject);
        }
    }
}