using deVoid.Utils;
using System.Collections.Generic;
using UnityEngine;
using static SpaceInvaders.AlienSpawner;

namespace SpaceInvaders
{
    public class AlienGroupController : MonoBehaviour
    {
        private int aliensMoved = 0;
        private Boundary _boundary;

        SpawnedAliens spawnedAliens;
        private float alienShootFrequency = 2.0f; // Average time (in seconds) between alien shots
        private float shootingTimer = 0f; // The next time when an alien will shoot
        Alien nextShootingAlien;
        bool aliensReachedBoundary = false;
        int currentAlienCount;

        void Start()
        {
            Signals.Get<Project.Game.MoveAlienSignal>().Dispatch();
        }
        private void OnEnable()
        {
            Signals.Get<Project.Game.MoveAlienCompletedSignal>().AddListener(OnMoveAlienCompleted);
            Signals.Get<Project.Game.AlienReachedBoundarySignal>().AddListener(OnAlienReachedBoundary);
            Signals.Get<Project.Game.AliensSpawnedSignal>().AddListener(OnAliensSpawned);
            Signals.Get<Project.SceneManager.ResetGameSignal>().AddListener(OnResetGame);
            Signals.Get<Project.Game.AlienKilledSignal>().AddListener(OnAlienKilled);
        }
        private void OnDisable()
        {
            Signals.Get<Project.Game.MoveAlienCompletedSignal>().RemoveListener(OnMoveAlienCompleted);
            Signals.Get<Project.Game.AlienReachedBoundarySignal>().RemoveListener(OnAlienReachedBoundary);
            Signals.Get<Project.Game.AliensSpawnedSignal>().RemoveListener(OnAliensSpawned);
            Signals.Get<Project.SceneManager.ResetGameSignal>().RemoveListener(OnResetGame);
            Signals.Get<Project.Game.AlienKilledSignal>().RemoveListener(OnAlienKilled);
        }
        private void OnAliensSpawned(SpawnedAliens spawnedAliens)
        {
            this.spawnedAliens = spawnedAliens;
            currentAlienCount = spawnedAliens.alienCount;
        }
        private float CalculateSpeedConstant()
        {
            float speed =  1 - Mathf.Lerp(1, 0, (float)currentAlienCount / spawnedAliens.alienCount);
            return speed;
        }
        private void OnAlienKilled(Alien alien)
        {
            //spawnedAliens.alienCount these are the starting amount of aliens.
            currentAlienCount--;
            //Recalculate the speed constant, we start the constant at 1 and as it gets closer to 0, the faster the aliens will go
            Signals.Get<Project.Game.SetSpeedSignal>().Dispatch(CalculateSpeedConstant());
        }
        private void OnMoveAlienCompleted()
        {
            aliensMoved++;
            if (aliensMoved >= currentAlienCount)
            {
                if (aliensReachedBoundary)
                {
                    Signals.Get<Project.Game.DirectionReversedSignal>().Dispatch();
                    aliensReachedBoundary = false;
                }
                else
                {
                    Signals.Get<Project.Game.MoveAlienSignal>().Dispatch();
                }
                aliensMoved = 0;
            }
        }
        private void OnAlienReachedBoundary(Boundary boundary)
        {
            if (boundary != null && _boundary != boundary)
            {
                aliensReachedBoundary = true;
                _boundary = boundary;
            }
        }
        void Update()
        {
            HandleAlienShooting();
        }
        private void HandleAlienShooting()
        {
            if (spawnedAliens == null)
                return;

            shootingTimer -= Time.deltaTime;

            if (shootingTimer <= 0)
            {
                // If we haven't chosen the next shooting alien yet, select one now
                if (nextShootingAlien == null)
                {
                    int randomColumnIndex = RandomRangeSeeded.Generate(0, spawnedAliens.aliensInColumns.Count);
                    nextShootingAlien = GetShootingAlienFromColumn(randomColumnIndex);

                    if (nextShootingAlien)
                    {
                        // Reset the timer to the selected alien's shooting interval
                        shootingTimer = nextShootingAlien.shootingInterval;
                    }
                }
                else
                {
                    // If we've already chosen the next shooting alien, let it shoot now
                    nextShootingAlien.Shoot();
                    nextShootingAlien = null;  // Reset for the next cycle
                }
            }
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

        public void OnResetGame()
        {
            _boundary = null;
            aliensMoved = 0;
            nextShootingAlien = null;
            spawnedAliens = null;
            shootingTimer = 0;
            aliensReachedBoundary = false; 
        }
    }
}