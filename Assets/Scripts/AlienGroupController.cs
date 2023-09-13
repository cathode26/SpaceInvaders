using deVoid.Utils;
using UnityEngine;

public class AlienGroupController : MonoBehaviour
{
    private int alienCount = 10;
    private int aliensMoved = 0;
    private Boundary _boundary;

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
}
