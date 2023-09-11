using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Harpnet.Carmine {
    public class InputManager : Singleton<InputManager> {
        [Header("Input Actions")]
        public InputActionProperty moveAction;
        public InputActionProperty lookAction;
        public InputActionProperty fireAction;
        public InputActionProperty jumpAction;

        [Header("Player Settings")]
        [Tooltip("Sensitivity multiplier for moving the camera arouind")]
        public float lookSensitivity = 1.0f;

        [Tooltip("Used to flip the vertical input axis")]
        public bool invertYAxis = false;

        [Tooltip("Used to flip the horizontal input axis")]
        public bool invertXAxis = false;

        private Player m_PlayerController;
        private bool m_FireInputWasHeld;

        private void OnEnable() {
            moveAction.action.Enable();
            lookAction.action.Enable();
            jumpAction.action.Enable();
            fireAction.action.Enable();
        }

        private void OnDisable() {
            moveAction.action.Disable();
            lookAction.action.Disable();
            jumpAction.action.Disable();
            fireAction.action.Disable();
        }

        private void Start() {
            m_PlayerController = GetComponent<Player>();

            // Lock Cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void LateUpdate() {
            //m_FireInputWasHeld = GetFireInputHeld();
        }

        public bool CanProcessInput() {
            return Cursor.lockState == CursorLockMode.Locked;
        }

        public Vector3 GetMoveInput() {
            Vector3 move = new Vector3(moveAction.action.ReadValue<Vector2>().x, 0.0f, moveAction.action.ReadValue<Vector2>().y);

            // Normalize Movement
            move = Vector3.ClampMagnitude(move, 1);

            return move;
        }

        public float GetLookInputHorizontal() {
            return GetMouseOrStickLookAxis().x;
        }

        public float GetLookInputVertical() {
            return -GetMouseOrStickLookAxis().y;
        }

        public bool GetJumpInputDown() {
            return jumpAction.action.triggered;
        }

        private Vector2 GetMouseOrStickLookAxis() {
            // Check if this look input is coming from the mouse or the gamepad
            // TODO: Idk how to do this yet
            bool isGamepad = false;
            Vector2 i = lookAction.action.ReadValue<Vector2>();

            // Invert horizontal input
            if(invertXAxis)
                i.x *= -1.0f;

            // Invert vertical input
            if(invertYAxis)
                i.y *= -1.0f;

            // Apply Sensitivity Mulitplier
            i *= lookSensitivity;

            if(isGamepad)
                i *= Time.deltaTime;
            else
                i *= 0.01f; // Reduce mouse input to be equivalent to stick movement

            return i;
        }
    }
}