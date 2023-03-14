// Unity
using UnityEngine;

namespace GUPS.EasyParallelScene.Demo
{
    public class CameraFollow : MonoBehaviour
    {
        /// <summary>
        /// The target that the camera will be following.
        /// </summary>
        private Transform target;

        /// <summary>
        /// The speed with which the camera will be following.
        /// </summary>
        public float smoothing = 5f;

        /// <summary>
        /// The initial offset from the target.
        /// </summary>
        private Vector3 offset;

        private void Awake()
        {
            this.target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Start ()
        {
            // Calculate the initial offset.
            offset = transform.position - target.position;
        }

        private void FixedUpdate ()
        {
            // Create a postion the camera is aiming for based on the offset from the target.
            Vector3 targetCamPos = target.position + offset;

            // Smoothly interpolate between the camera's current position and it's target position.
            transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}