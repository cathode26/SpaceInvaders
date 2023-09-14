using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpaceInvaders.PrefabTypes;

namespace SpaceInvaders
{
    public class ObjectPooler : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public SpawnableType type;
            public GameObject prefab;
            public int initialSize;
        }

        public static ObjectPooler Instance;

        public List<Pool> pools;
        private Dictionary<SpawnableType, Queue<GameObject>> poolDictionary;
        private Dictionary<(SpawnableType, GameObject), Coroutine> activeReturnCoroutines = new Dictionary<(SpawnableType, GameObject), Coroutine>();

        private void Awake()
        {
            Instance = this;
            poolDictionary = new Dictionary<SpawnableType, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                for (int i = 0; i < pool.initialSize; i++)
                {
                    GameObject obj = Instantiate(pool.prefab, transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary.Add(pool.type, objectPool);
            }
        }
        public GameObject RequestObject(SpawnableType spawnableType, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(spawnableType))
            {
                Debug.LogWarning("Pool with type " + spawnableType + " doesn't exist.");
                return null;
            }

            if (poolDictionary[spawnableType].Count == 0)
            {
                // Create a new instance if the pool is empty
                Pool pool = pools.Find(p => p.type == spawnableType);
                GameObject newObj = Instantiate(pool.prefab, transform);
                poolDictionary[spawnableType].Enqueue(newObj);
            }

            GameObject objectToSpawn = poolDictionary[spawnableType].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            return objectToSpawn;
        }
        
        // Modified ReturnAfterDelay
        private IEnumerator ReturnAfterDelay(SpawnableType type, GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);

            activeReturnCoroutines.Remove((type,obj));  // Remove from active coroutines when done

            obj.SetActive(false);
            poolDictionary[type].Enqueue(obj);
        }

        // Modified ReturnObject
        public void ReturnObject(SpawnableType type, GameObject obj, float delay = 0f)
        {
            if (delay > 0f)
            {
                // If there's an active coroutine for this object, stop it first
                if (activeReturnCoroutines.ContainsKey((type,obj)))
                {
                    StopCoroutine(activeReturnCoroutines[(type,obj)]);
                    activeReturnCoroutines.Remove((type, obj));
                }

                // Start the coroutine and store it in the dictionary
                Coroutine coroutine = StartCoroutine(ReturnAfterDelay(type, obj, delay));
                activeReturnCoroutines.Add((type,obj), coroutine);
            }
            else
            {
                obj.SetActive(false);
                poolDictionary[type].Enqueue(obj);
            }
        }

        // New method to immediately return all objects being delayed
        public void ImmediateReturnAllDelayedObjects()
        {
            foreach (var entry in activeReturnCoroutines)
            {
                (SpawnableType type, GameObject obj) pair = entry.Key;
                StopCoroutine(entry.Value);  // Stop the delay coroutine

                pair.obj.SetActive(false);
                poolDictionary[pair.type].Enqueue(pair.obj);
            }

            activeReturnCoroutines.Clear();
        }

        public BoxCollider GetBoxCollider(SpawnableType spawnableType)
        {
            Pool pool = pools.Find( obj => obj.type == spawnableType );
            return pool.prefab.GetComponent<BoxCollider>();
        }
    }
}