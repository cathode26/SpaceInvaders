using UnityEngine;

public class AlienSpawner : MonoBehaviour
{
    public GameObject[] alienPrefabs; 

    public Transform leftPosition;
    public Transform rightPosition;
    public Transform topPosition;
    public Transform bottomPosition;

    public float alienHeightOffsetPercentage = 0.1f; // The percentage of the max alien height to offset from the top
    public int numberOfRows = 10;
    public int aliensPerRow = 10;

    private float maxAlienWidth;
    private float maxAlienHeight;

    void Start()
    {
        CalculateMaxAlienSize();
        SpawnAliens();
    }

    void CalculateMaxAlienSize()
    {
        foreach (GameObject alienPrefab in alienPrefabs)
        {
            BoxCollider alienCollider = alienPrefab.GetComponent<BoxCollider>();
            if (alienCollider)
            {
                maxAlienWidth = Mathf.Max(maxAlienWidth, alienCollider.size.x);
                maxAlienHeight = Mathf.Max(maxAlienHeight, alienCollider.size.y);
            }
            else
            {
                Debug.LogError($"Alien prefab {alienPrefab.name} does not have a BoxCollider!");
            }
        }
    }

    void SpawnAliens()
    {
        float totalWidth = (aliensPerRow - 1) * maxAlienWidth;
        float startX = (leftPosition.position.x + rightPosition.position.x - totalWidth) / 2;
        float startY = topPosition.position.y - (maxAlienHeight * alienHeightOffsetPercentage);

        for (int row = 0; row < numberOfRows; row++)
        {
            // Choose a random alien type for this row
            GameObject alienPrefab = alienPrefabs[Random.Range(0, alienPrefabs.Length)];

            // Create a new parent for this row
            GameObject rowParent = new GameObject($"Row_{row + 1}");
            rowParent.transform.SetParent(this.transform);
            rowParent.transform.localPosition = Vector3.zero;

            for (int col = 0; col < aliensPerRow; col++)
            {
                Vector3 spawnPosition = new Vector3(startX + (col * maxAlienWidth), startY - (row * maxAlienHeight), 0);
                GameObject alien = Instantiate(alienPrefab, spawnPosition, Quaternion.identity, rowParent.transform);
                alien.transform.localPosition = new Vector3(alien.transform.localPosition.x, alien.transform.localPosition.y, 0.0f);
            }
        }
    }
}
