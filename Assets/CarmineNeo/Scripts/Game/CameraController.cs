using UnityEngine;
using UnityEngine.InputSystem;

namespace Harpnet.Carmine {
    public class CameraController : MonoBehaviour {
        [SerializeField] private float sensitivityX;
        [SerializeField] private float sensitivityY;

        private Camera cam = null;

        private float mouseX = 0.0f;
        private float mouseY = 0.0f;

        private float multiplier = 0.01f;

        private float rotationX = 0.0f;
        private float rotationY = 0.0f;

        private void Start() {
            // Get Camera
            cam = GetComponentInChildren<Camera>();

            // Set Cursor State
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update() {
            cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.localRotation = Quaternion.Euler(0, rotationY, 0);
        }

        private void OnLook(InputValue value) {
            mouseX = value.Get<Vector2>().x;
            mouseY = value.Get<Vector2>().y;

            rotationY += mouseX * sensitivityX * multiplier;
            rotationX -= mouseY * sensitivityY * multiplier;

            rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        }
    }
}