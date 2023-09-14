using SpaceInvaders;
using UnityEngine;

public class UFOSpawner : MonoBehaviour
{
    public float minUfoSpawnTime = 10f;
    public float maxUfoSpawnTime = 20f;

    private float ufoSpawnCounter;
    private float nextUfoSpawnTime;

    private void Start()
    {
        SetNextUfoSpawnTime();
    }

    void Update()
    {
        ufoSpawnCounter += Time.deltaTime;

        if (ufoSpawnCounter >= nextUfoSpawnTime)
        {
            SpawnUFO();
            SetNextUfoSpawnTime();
            ufoSpawnCounter = 0; // Reset the counter
        }
    }

    void SetNextUfoSpawnTime()
    {
        nextUfoSpawnTime = Random.Range(minUfoSpawnTime, maxUfoSpawnTime);
    }

    void SpawnUFO()
    {
        UFOAlien newAlien = ObjectPooler.Instance.RequestObject(PrefabTypes.SpawnableType.UFOAlien, Vector3.zero, Quaternion.identity).GetComponent<UFOAlien>();

        newAlien.transform.SetParent(transform);
        // Randomly decide the starting direction
        bool isMovingRight = (Random.value > 0.5f);
        newAlien.SetDirection(isMovingRight);
        if (isMovingRight)
        {
            newAlien.transform.position = new Vector3(-9, transform.position.y + 2.5f, transform.position.z); // Assume starting just off-screen on the left
        }
        else
        {
            newAlien.transform.position = new Vector3(9, transform.position.y + 2.5f, transform.position.z); // Assume starting just off-screen on the right
        }

        Debug.Log("newAlien " + newAlien.transform.position);
    }
}
