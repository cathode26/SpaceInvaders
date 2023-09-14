using SpaceInvaders;
using UnityEngine;
using static SpaceInvaders.PrefabTypes;

public class EnemyBullet : Bullet
{
    private void Awake()
    {
        direction = Vector3.down;
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