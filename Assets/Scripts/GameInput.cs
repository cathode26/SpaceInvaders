using deVoid.Utils;
using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class GameInput : MonoBehaviour // Singleton class responsible for managing game input, including key bindings and player interactions
    {
        public static GameInput Instance { get; private set; } // Singleton instance of the GameInput class
        [SerializeField]
        private PlayerInputActions playerInputActions;
        private bool escapeEnabled = true;
        //EventHandler is the built in C# standard for delegate
        //Event that is triggered when the player performs the interaction action (presses "E").
        public event EventHandler OnInteractAction;
        //Event that is triggered when the player performs the interaction action (presses "F").
        public event EventHandler OnInteractAlternativeAction;
        public event EventHandler OnPauseAction;
        private void Awake()
        {
            Instance = this;
            playerInputActions = new PlayerInputActions();

            playerInputActions.Player.Enable();

            //Here, the input system is initialized and the Interact action is set up to trigger the OnInteractAction event when performed.
            playerInputActions.Player.Interact.performed += InteractPerformed;

            playerInputActions.Player.Pause.performed += Pause_performed;
            Signals.Get<Project.Input.OnEnableEscapeSignal>().AddListener(OnEnableEscape);

        }
        private void OnDestroy()
        {
            playerInputActions.Player.Interact.performed -= InteractPerformed;
            playerInputActions.Player.Pause.performed -= Pause_performed;
            playerInputActions.Dispose();
            Signals.Get<Project.Input.OnEnableEscapeSignal>().AddListener(OnEnableEscape);
        }
        private void InteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            //? null condition operator, followed by Invoke, because you can't put the function parenthesis after the ? null condition operator
            OnInteractAction?.Invoke(this, EventArgs.Empty);
        }
        public (bool, Vector2) GetMovementVectorNormalized()
        {
            //This method retrieves the direction of the movement from the input system and checks whether the movement action is currently being performed (i.e., the movement keys are being pressed).
            Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
            bool moved = playerInputActions.Player.Move.IsPressed();
            return (moved, inputVector);
        }
        private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if(escapeEnabled)
                OnPauseAction?.Invoke(this, EventArgs.Empty);
        }
        private void OnEnableEscape(bool enable)
        {
            escapeEnabled = enable;
        }
    }
}