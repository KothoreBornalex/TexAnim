using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TexAnim
{
    public class CameraController : MonoBehaviour
    {
        public float moveSpeed = 10f;          // Speed of camera movement
        public float rotationSpeed = 2f;       // Speed of camera rotation
        public float zoomSpeed = 5f;           // Speed of zoom (FOV adjustment)
        public float minFOV = 10f;             // Minimum FOV
        public float maxFOV = 60f;             // Maximum FOV

        private Vector3 lastPosition;
        private float currentFOV;

        private void Start()
        {
            currentFOV = Camera.main.fieldOfView;
            lastPosition = transform.position;
        }

        private void Update()
        {
            HandleMovement();
            HandleRotation();
            HandleZoom();
        }

        private void HandleMovement()
        {
            float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

            transform.Translate(moveX, 0, moveY);
        }

        private void HandleRotation()
        {
            if (Input.GetMouseButton(1))  // Right mouse button to rotate
            {
                float rotX = Input.GetAxis("Mouse X") * rotationSpeed;
                float rotY = -Input.GetAxis("Mouse Y") * rotationSpeed;

                transform.RotateAround(transform.position, Vector3.up, rotX);     // Rotate horizontally
                transform.RotateAround(transform.position, transform.right, rotY); // Rotate vertically
            }
        }

        private void HandleZoom()
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (scrollInput != 0f)
            {
                currentFOV -= scrollInput * zoomSpeed * 100f;  // Adjust zoom sensitivity
                currentFOV = Mathf.Clamp(currentFOV, minFOV, maxFOV);
                Camera.main.fieldOfView = currentFOV;
            }
        }
    }
}
