using deVoid.Utils;
using System.Collections.Generic;
using UnityEngine;
using static SpaceInvaders.PrefabTypes;

namespace SpaceInvaders
{
    public class AlienSpawner : MonoBehaviour
    {
        public SpawnableType[] alienTypes;

        public Transform leftPosition;
        public Transform rightPosition;
        public Transform topPosition;
        public Transform bottomPosition;

        public float alienHeightOffsetPercentage = 0.1f; // The percentage of the max alien height to offset from the top
        public float alienWidthOffsetPercentage = 0.3f; // The percentage of the max alien height to offset from the top

        public int numberOfRows = 10;
        public int numberOfColumns = 10;

        public LevelData[] levels;
        private int currentLevelIndex = 0;

        public class SpawnedAliens
        {
            public int alienCount;
            public List<List<Alien>> aliensInRows;
            public List<List<Alien>> aliensInColumns;
            public SpawnedAliens(List<List<Alien>> aliensInRows, List<List<Alien>> aliensInColumns, int alienCount)
            {
                this.alienCount = alienCount;
                this.aliensInRows = aliensInRows;
                this.aliensInColumns = aliensInColumns;
            }
        };
        private SpawnedAliens spawnedAliens;

        private float maxAlienWidth;
        private float maxAlienHeight;

        private void Awake()
        {
            Signals.Get<Project.Game.LoadGameSignal>().AddListener(OnLoadGame);
            Signals.Get<Project.Game.ResetGameSignal>().AddListener(OnResetGame);
            Signals.Get<Project.Game.LoadNextLevelSignal>().AddListener(LoadNextLevelSignal);
        }
        private void OnDestroy()
        {
            Signals.Get<Project.Game.LoadGameSignal>().RemoveListener(OnLoadGame);
            Signals.Get<Project.Game.ResetGameSignal>().RemoveListener(OnResetGame);
            Signals.Get<Project.Game.LoadNextLevelSignal>().RemoveListener(LoadNextLevelSignal);
        }
        private void LoadNextLevelSignal()
        {
            currentLevelIndex++;
            OnLoadGame();
        }
        private void OnLoadGame()
        {
            if (currentLevelIndex >= levels.Length)
                currentLevelIndex = 0;

            if (currentLevelIndex < levels.Length)
            {
                // Use the currentLevel data to spawn and place your aliens.
                LevelData currentLevel = levels[currentLevelIndex];
                alienTypes = currentLevel.enemiesList.ToArray();
                numberOfRows = currentLevel.numberOfRows;
                numberOfColumns = currentLevel.numberOfColumns;

                CalculateMaxAlienSize();
                SpawnAliens();
            }
        }
        void CalculateMaxAlienSize()
        {
            maxAlienWidth = 0.0f;
            maxAlienHeight = 0.0f;
            foreach (SpawnableType alienType in alienTypes)
            {
                BoxCollider alienCollider = ObjectPooler.Instance.GetBoxCollider(alienType);
                if (alienCollider)
                {
                    maxAlienWidth = Mathf.Max(maxAlienWidth, alienCollider.size.x);
                    maxAlienHeight = Mathf.Max(maxAlienHeight, alienCollider.size.y);
                }
                else
                {
                    Debug.LogError($"Alien prefab {alienType.ToString()} does not have a BoxCollider!");
                }
            }
            maxAlienWidth = maxAlienWidth + (maxAlienWidth * alienWidthOffsetPercentage);
        }
        public void SpawnAliens()
        {
            if (currentLevelIndex >= levels.Length)
                currentLevelIndex = 0;

            if (currentLevelIndex < levels.Length)
            {
                // Use the currentLevel data to spawn and place your aliens.
                float totalWidth = (numberOfColumns - 1) * maxAlienWidth;
                float startX = (leftPosition.position.x + rightPosition.position.x - totalWidth) / 2;
                float startY = topPosition.position.y - (maxAlienHeight * alienHeightOffsetPercentage) - (currentLevelIndex * 0.25f);
                List<List<Alien>> aliensInRows = new List<List<Alien>>();
                List<List<Alien>> aliensInColumns = new List<List<Alien>>();

                for (int row = 0; row < numberOfRows; row++)
                {
                    // Determine the type of alien for this row
                    SpawnableType alienType = alienTypes[Random.Range(0, alienTypes.Length)];

                    List<Alien> currentRowAliens = new List<Alien>();
                    for (int col = 0; col < numberOfColumns; col++)
                    {
                        Alien newAlien = ObjectPooler.Instance.RequestObject(alienType, Vector3.zero, Quaternion.identity).GetComponent<Alien>();
                        newAlien.transform.SetParent(transform);

                        // Calculate position based on row, column, and desired spacing
                        Vector3 spawnPosition = new Vector3(startX + (col * maxAlienWidth), startY - (row * maxAlienHeight), 0);

                        newAlien.transform.localPosition = spawnPosition;

                        currentRowAliens.Add(newAlien);

                        // Add to columns list
                        if (aliensInColumns.Count <= col)
                        {
                            aliensInColumns.Add(new List<Alien>());
                        }
                        aliensInColumns[col].Add(newAlien);
                    }
                    aliensInRows.Add(currentRowAliens);
                }

                spawnedAliens = new SpawnedAliens(aliensInRows, aliensInColumns, numberOfColumns * numberOfRows);
                Signals.Get<Project.Game.AliensSpawnedSignal>().Dispatch(spawnedAliens);
            }
        }
        private void OnResetGame()
        {
            foreach (List<Alien> alienList in spawnedAliens.aliensInRows)
                foreach (Alien alien in alienList)
                    if (alien.IsAlive)
                        alien.Kill();

            spawnedAliens = null;
            currentLevelIndex = 0;
        }
    }
}