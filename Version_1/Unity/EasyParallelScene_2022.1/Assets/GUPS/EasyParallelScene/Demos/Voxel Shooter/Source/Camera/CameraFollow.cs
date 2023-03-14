/// Unity
using UnityEngine;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demos.VoxelShooter
{
    /// <summary>
    /// This class is used to make the Camera follow the Player.
    /// Smoothly interpolate between the current Camera location and Player location, while using the ParallelScene set DeltaTime.
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        /// <summary>
        /// The speed with which the Camera will be following.
        /// </summary>
        public float smoothing = 5f;

        /// <summary>
        /// The target that the Camera will be following.
        /// </summary>
        private Transform target;

        /// <summary>
        /// The initial offset from the target.
        /// </summary>
        private Vector3 offset;

        private void Awake()
        {
            // Get the Transform of the Player.
            this.target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Start ()
        {
            // Calculate the initial offset.
            this.offset = this.transform.position - this.target.position;
        }

        private void FixedUpdate ()
        {
            // Create a postion the Camera is aiming for based on the offset from the target.
            Vector3 targetCamPos = this.target.position + this.offset;

            // Smoothly interpolate between the Camera's current position and it's target position, using the through EasyParallelScene assigned DeltaTime.
            this.transform.position = Vector3.Lerp (this.transform.position, targetCamPos, this.smoothing * this.GetSceneDeltaTime());
        }
    }
}