using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TexAnim
{
    public class TexAnimLOD : MonoBehaviour
    {
        public Shader staticShader;  // Assign your static shader in the inspector
        public Shader animatedShader; // Assign your animated shader in the inspector
        public float distanceThreshold = 10f;  // Distance threshold for switching shaders

        private Renderer _renderer;
        private Camera _camera;

        void Start()
        {
            _renderer = GetComponent<Renderer>();
            _camera = Camera.main;  // Reference the main camera
        }

        void Update()
        {
            // Calculate distance from the camera to the object
            float distance = Vector3.Distance(_camera.transform.position, transform.position);

            // Switch shader based on distance
            if (distance > distanceThreshold)
            {
                _renderer.material.shader = staticShader;
            }
            else
            {
                _renderer.material.shader = animatedShader;
            }
        }
    }
}
