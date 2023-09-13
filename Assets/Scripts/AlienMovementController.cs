using deVoid.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class AlienMovementController : MonoBehaviour
    {
        enum AlienMoveState
        {
            STATIONARY,
            INITIAL_PAUSE,
            MOVING_HORIZONTAL,
            MOVING_DOWN,
            FINAL_PAUSE
        }

        private bool _isAlive = true;
        private AlienMoveState _currentState = AlienMoveState.STATIONARY;
        private int _moveDirection = -1;
        private float _moveDistance = 0.010f;  // Distance invaders move in each "step"
        private float _curMoveTime = 0.0f;
        private int _moveDuration = 100;
        private Vector3 _startingPosition;
        private float _downDistance = 0.05f;  // Distance invaders move down when changing direction
        private int _pauseDuration = 100;  // ms Time between each movement "step"
        private float _curPauseTime = 0.0f;

        private float _startMoveDuration = 0.0f;
        private float _curStartMoveTime = 0.0f;

        private int _downMoveDuration = 200;
        private float _curDownMoveTime = 0.0f;

        private int _initialPauseDuration = 500;  // ms Store the initial pause duration for speed increase logic

        public bool IsAlive() { return _isAlive; }

        private List<AlienMovementController> _siblingAliens;
        private int _ownIndex;

        private void Start()
        {
            _currentState = AlienMoveState.STATIONARY;
            _startingPosition = transform.position;
            _startMoveDuration = RandomRangeSeeded.Generate(0, _pauseDuration - _moveDuration);
        }
        private void OnEnable()
        {
            GatherSiblingsAndSetOwnIndex();
            Signals.Get<Project.Game.MoveAlienSignal>().AddListener(OnMoveAlien);
            Signals.Get<Project.Game.DirectionReversedSignal>().AddListener(OnDirectionReversed);
        }
        private void OnDisable()
        {
            Signals.Get<Project.Game.MoveAlienSignal>().RemoveListener(OnMoveAlien);
            Signals.Get<Project.Game.DirectionReversedSignal>().RemoveListener(OnDirectionReversed);
        }
        private void Update()
        {
            if (_currentState == AlienMoveState.INITIAL_PAUSE)
            {
                _curStartMoveTime += Time.deltaTime;
                _curPauseTime = _curStartMoveTime;
                if (_curStartMoveTime >= _startMoveDuration / 1000.0f)
                    _currentState = AlienMoveState.MOVING_HORIZONTAL;
            }
            else if (_currentState == AlienMoveState.MOVING_HORIZONTAL)
            {
                _curMoveTime += Time.deltaTime;
                _curPauseTime = _curStartMoveTime;
                if (_curMoveTime >= _moveDuration / 1000.0f)
                {
                    _currentState = AlienMoveState.FINAL_PAUSE;
                    _curMoveTime = _moveDuration / 1000.0f;
                }
                float distance = (_curMoveTime / (_moveDuration / 1000.0f)) * _moveDirection * _moveDistance;
                transform.position = new Vector3(_startingPosition.x + distance, _startingPosition.y, _startingPosition.z);
            }
            else if(_currentState == AlienMoveState.MOVING_DOWN)
            {
                _curDownMoveTime += Time.deltaTime;
                _curPauseTime = _curDownMoveTime;

                if (_curDownMoveTime >= _downMoveDuration / 1000.0f)
                {
                    _currentState = AlienMoveState.FINAL_PAUSE;
                    _curDownMoveTime = _downMoveDuration / 1000.0f;
                }
                float distance = (_curDownMoveTime / (_downMoveDuration / 1000.0f)) * _downDistance;
                transform.position = new Vector3(_startingPosition.x, _startingPosition.y - distance, _startingPosition.z);
            }
            else
            {
                _curPauseTime += Time.deltaTime;
                if (_curPauseTime >= _pauseDuration / 1000.0f)
                {
                    _currentState = AlienMoveState.STATIONARY;
                    Signals.Get<Project.Game.MoveAlienCompletedSignal>().Dispatch();
                }
            }
        }
        private void OnTriggerEnter(Collider collider)
        {
            Boundary boundary = collider.gameObject.GetComponent<Boundary>();
            if (boundary != null)
                Signals.Get<Project.Game.OnAlienReachedBoundarySignal>().Dispatch(boundary);
        }

        private void OnMoveAlien()
        {
            _curPauseTime = 0.0f;
            _curStartMoveTime = 0.0f;
            _curMoveTime = 0.0f;
            _startingPosition = transform.position;
            _currentState = AlienMoveState.INITIAL_PAUSE;
            _startMoveDuration = RandomRangeSeeded.Generate(0, _pauseDuration - _moveDuration);

        }
        private void OnDirectionReversed()
        {
            _curDownMoveTime = 0.0f;
            _moveDirection *= -1;
            _startingPosition = transform.position;
            _currentState = AlienMoveState.MOVING_DOWN;
        }
        public AlienMovementController GetClosestActiveAlienAndOptimize(int startIndex, int direction, List<AlienMovementController> alienList)
        {
            int currentIndex = startIndex + direction;  // Start looking in the given direction.

            while (currentIndex >= 0 && currentIndex < alienList.Count)
            {
                if (alienList[currentIndex].IsAlive())
                {
                    // Swap the current alien with its active neighbor for future optimization.
                    var temp = alienList[startIndex];
                    alienList[startIndex] = alienList[currentIndex];
                    alienList[currentIndex] = temp;

                    return alienList[startIndex];
                }
                currentIndex += direction;
            }

            return null;  // No active alien found in the given direction.
        }
        private void GatherSiblingsAndSetOwnIndex()
        {
            _siblingAliens = new List<AlienMovementController>(transform.parent.GetComponentsInChildren<AlienMovementController>());
            _ownIndex = _siblingAliens.IndexOf(this);
        }
    }
}