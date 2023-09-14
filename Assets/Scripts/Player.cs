using deVoid.Utils;
using UnityEngine;
using static SpaceInvaders.PrefabTypes;

namespace SpaceInvaders
{
    public class Player : MonoBehaviour
    {
        // Singleton instance to allow other classes easy access to Player's properties and methods.
        public static Player Instance { get; private set; }

        [SerializeField]
        private float movementSpeed = 5.0f;
        [SerializeField]
        private GameInput gameInput; // Reference to the game input system.
        [Header("Shooting")]
        [SerializeField]
        private GameObject bulletPrefab;
        [SerializeField]
        private Transform bulletSpawnLocation;

        private bool isMoving = false; // Flag to track player's movement state.
        private BoxCollider _boxCollider; // Reference to the player's box collider.

        // Event triggered when player's moving state changes.
        public delegate void MovingState(bool isWalking);
        public event MovingState OnMovingStateChanged;
        private int playerLives = 3; // player's initial lives
        private Vector3 startingPosition;
        private int startingLives;

        private void Awake()
        {
            // Singleton pattern setup.
            if (Instance == null)
                Instance = this;
            else
                Debug.Log("Error: Instance of Player is not null");

            startingLives = playerLives;
            startingPosition = transform.position;
            _boxCollider = GetComponent<BoxCollider>();
            Signals.Get<Project.SceneManager.ResetGameSignal>().AddListener(OnResetGame);
        }
        private void OnDestroy()
        {
            Signals.Get<Project.SceneManager.ResetGameSignal>().RemoveListener(OnResetGame);
        }
        private void OnResetGame()
        {
            transform.position = startingPosition;
            playerLives = startingLives;
        }
        // Register and unregister events.
        private void OnEnable()
        {
            gameInput.OnInteractAction += GameInputOnInteractAction;
        }
        private void OnDisable()
        {
            gameInput.OnInteractAction -= GameInputOnInteractAction;
        }
        //  The GameInputOnInteractAction event is called from the GameInput when the user presses space
        private void GameInputOnInteractAction(object sender, System.EventArgs e)
        {
            //Check for null operator ?
            //selectedCounter?.Interact(this);
            //Shoot the weapon
            HandleShoot();
        }
        private void Update()
        {
            //Handles player movement according to user input.
            HandleMovement();
        }
        private void HandleShoot()
        {
            GameObject bullet = ObjectPooler.Instance.RequestObject(SpawnableType.PlayerBullet, bulletSpawnLocation.position, Quaternion.identity);
            bullet.SetActive(true);
        }
        // Handle player's movements based on user input.
        private void HandleMovement()
        {
            //Controls the movement and rotation of the player according to user input.
            (bool moved, Vector2 movDir) movementData = gameInput.GetMovementVectorNormalized();

            if (movementData.moved)
            {
                bool canMove = DetermineMovementAbility(movementData.movDir);
                if (canMove)
                {
                    Vector3 movDir = new Vector3(movementData.movDir.x, 0.0f, 0.0f);
                    transform.position += movDir * Time.deltaTime;/* * adjustedSpeed;*/
                    isMoving = true;
                }
                else
                {
                    if (isMoving == true)
                        OnMovingStateChanged?.Invoke(false);
                    isMoving = false;
                }
            }
            else
            {
                if (isMoving == true)
                    OnMovingStateChanged?.Invoke(false);
                isMoving = false;
            }
        }
        /* 
         * Input: the input vector from unity's new input system
         * Output: a bool indicating if the player can move
         * 
         * This function will attempt to find a valid direction that the player can travel by 
         * 1) Creating a box around the player
         * 2) Checking if the box when moved in the direction of the input vector, collides with a wall 
         */
        private bool DetermineMovementAbility(Vector2 inputVector)
        {
            Vector3 movDir = new Vector3(inputVector.x, 0f, inputVector.y);
            float moveDistance = movementSpeed * Time.deltaTime;

            Vector3 halfExtents = _boxCollider.size * 0.5f; // half the size of the box collider
            Vector3 origin = transform.position + _boxCollider.center; // adjust for the center offset of the box collider

            // Only consider objects on the "Wall" layer for the BoxCast
            int wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
            RaycastHit[] hitInfos = Physics.BoxCastAll(origin, halfExtents, movDir, transform.rotation, moveDistance, wallLayerMask);

            // If no collisions were detected, return true to allow movement
            if (hitInfos.Length == 0)
                return true;

            foreach (var hit in hitInfos)
            {
                Boundary boundary = hit.transform.GetComponent<Boundary>();
                if (boundary != null)
                {
                    if (boundary.boundaryType == Boundary.BoundaryType.LeftBoundary && inputVector.x < 0)
                        return false; // trying to move left into the left wall
                    if (boundary.boundaryType == Boundary.BoundaryType.RightBoundary && inputVector.x > 0)
                        return false; // trying to move right into the right wall
                }
            }

            return true; // no problematic collisions detected
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check if the player is hit by an enemy bullet
            if (other.gameObject.GetComponent<EnemyBullet>())
            {
                // Decrease player's life
                DecreaseLife();
            }
            if (other.gameObject.GetComponent<Alien>())
                GameOver();
        }

        private void DecreaseLife()
        {
            playerLives--;

            // Notify about the change in player lives
            Signals.Get<Project.Game.LivesChangedSignal>().Dispatch(playerLives);
            // Check if player is out of lives
            if (playerLives <= 0)
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            // Pause the game
            
            Signals.Get<Project.Game.NoMoreLivesSignal>().Dispatch();

            // TODO: Show GameOver UI or any other related logic.
        }
    }
}