// Core
using System.Linq;

// Unity
using UnityEngine;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demos.VoxelShooter
{
    /// <summary>
    /// This class is used to move the player around the scene.
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        /// <summary>
        /// The speed that the player will move at.
        /// </summary>
        public float speed = 6f;

        /// <summary>
        /// Reference to the world Map.
        /// </summary>
        private Map map;

        private void Awake()
        {
            // Find the Map the Player is in.
            this.FindMap();
        }

        private void FixedUpdate ()
        {
            // Store the input axes.
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            // Move the player around the scene.
            this.Move (h, v);

            // Turn the player to face the mouse cursor.
            this.Turning ();
        }

        /// <summary>
        /// Find the Map the Player is in.
        /// </summary>
        public void FindMap()
        {
            // Set up the references.
            this.map = GameObjectHelper.FindGameObjectsWithLayer(this.gameObject.scene).Where(g => g.name == "Map").FirstOrDefault()?.GetComponent<Map>();
        }

        /// <summary>
        /// Move the player.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="v"></param>
        private void Move (float h, float v)
        {
            // Move only if this scene is in front.
            if (this.IsSceneActive())
            {
                // Set the movement vector based on the axis input.
                Vector3 var_Movement = new Vector3(h, 0f, v);

                // Normalise the movement vector and make it proportional to the speed per second.
                var_Movement = var_Movement.normalized * this.speed * this.GetSceneDeltaTime();

                Vector3 var_TargetPosition = this.transform.position + var_Movement;

                // Check if Block in front is empty and there is a block in front below.
                if (this.map.GetBlock(var_TargetPosition + Vector3.up / 2f + var_Movement.normalized / 2) == null)
                {
                    this.transform.position = var_TargetPosition;
                }
            }
        }

        /// <summary>
        /// The player will turn to face the mouse cursor.
        /// </summary>
        private void Turning ()
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit floorHit;

            // Perform the raycast and if it hits something on the floor layer...
            if (Physics.Raycast (camRay, out floorHit, 100f, this.GetSceneLayerMask()))
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = floorHit.point - this.transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation (playerToMouse);

                // Set the player's rotation to this new rotation.
                this.transform.rotation = newRotatation;
            }
        }
    }
}