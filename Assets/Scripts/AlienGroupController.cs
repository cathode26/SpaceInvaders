using deVoid.Utils;
using System.Collections.Generic;
using UnityEngine;
using static SpaceInvaders.AlienSpawner;

namespace SpaceInvaders
{
    public class AlienGroupController : MonoBehaviour
    {
        private int alienCount = 10;
        private int aliensMoved = 0;
        private Boundary _boundary;

        SpawnedAliens spawnedAliens;
        private float alienShootFrequency = 2.0f; // Average time (in seconds) between alien shots
        private float nextShootTime = 0f; // The next time when an alien will shoot
        
        void Start()
        {
            Signals.Get<Project.Game.MoveAlienSignal>().Dispatch();
        }
        private void OnEnable()
        {
            Signals.Get<Project.Game.MoveAlienCompletedSignal>().AddListener(OnMoveAlienCompleted);
            Signals.Get<Project.Game.OnAlienReachedBoundarySignal>().AddListener(OnAlienReachedBoundary);
            Signals.Get<Project.Game.AliensSpawnedSignal>().AddListener(OnAliensSpawned);
        }
        private void OnDisable()
        {
            Signals.Get<Project.Game.MoveAlienCompletedSignal>().RemoveListener(OnMoveAlienCompleted);
            Signals.Get<Project.Game.OnAlienReachedBoundarySignal>().RemoveListener(OnAlienReachedBoundary);
            Signals.Get<Project.Game.AliensSpawnedSignal>().RemoveListener(OnAliensSpawned);
        }
        private void OnAliensSpawned(SpawnedAliens spawnedAliens)
        {
            this.spawnedAliens = spawnedAliens;
        }
        private void OnMoveAlienCompleted()
        {
            aliensMoved++;
            if (alienCount >= aliensMoved)
            {
                Signals.Get<Project.Game.MoveAlienSignal>().Dispatch();
                aliensMoved = 0;
            }
        }
        private void OnAlienReachedBoundary(Boundary boundary)
        {
            if (boundary != null && _boundary != boundary)
            {
                _boundary = boundary;
                Signals.Get<Project.Game.DirectionReversedSignal>().Dispatch();
            }
        }
        void Update()
        {
            HandleAlienShooting();
        }
        private void HandleAlienShooting()
        {
            if (Time.time >= nextShootTime)
            {
                ShootFromRandomAlien();
                float variance = RandomRangeSeeded.Generate(-1000, 1000)/1000.0f; // Random variance to make shooting unpredictable
                nextShootTime = Time.time + alienShootFrequency + variance;
            }
        }
        private void ShootFromRandomAlien()
        {
            // Randomly select a column
            int randomColumnIndex = RandomRangeSeeded.Generate(0, spawnedAliens.aliensInColumns.Count);

            Alien shootingAlien = GetShootingAlienFromColumn(randomColumnIndex);

            // If there's no alive alien in the randomly selected column, return
            if (shootingAlien == null) return;

            shootingAlien.Shoot();
        }

        public Alien GetShootingAlienFromColumn(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= spawnedAliens.aliensInColumns.Count)
                return null;

            List<Alien> column = spawnedAliens.aliensInColumns[columnIndex];
            for (int i = column.Count - 1; i >= 0; i--)
            {
                if (column[i].IsAlive)  // Assuming there's an IsAlive property in Alien class
                {
                    return column[i];
                }
            }

            return null;  // No alive aliens in this column
        }
    }
}