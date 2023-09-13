using deVoid.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class AlienGroupController : MonoBehaviour
    {
        private int alienCount = 10;
        private int aliensMoved = 0;
        private Boundary _boundary;

        // Data structures for rows and columns
        private List<List<Alien>> horizontalRows = new List<List<Alien>>();
        private List<List<Alien>> verticalColumns = new List<List<Alien>>();

        void Start()
        {
            Signals.Get<Project.Game.MoveAlienSignal>().Dispatch();
        }
        private void OnEnable()
        {
            Signals.Get<Project.Game.MoveAlienCompletedSignal>().AddListener(OnMoveAlienCompleted);
            Signals.Get<Project.Game.OnAlienReachedBoundarySignal>().AddListener(OnAlienReachedBoundary);
        }
        private void OnDisable()
        {
            Signals.Get<Project.Game.MoveAlienCompletedSignal>().RemoveListener(OnMoveAlienCompleted);
            Signals.Get<Project.Game.OnAlienReachedBoundarySignal>().RemoveListener(OnAlienReachedBoundary);
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
        public Alien GetShootingAlienFromColumn(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= verticalColumns.Count)
                return null;

            List<Alien> column = verticalColumns[columnIndex];
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