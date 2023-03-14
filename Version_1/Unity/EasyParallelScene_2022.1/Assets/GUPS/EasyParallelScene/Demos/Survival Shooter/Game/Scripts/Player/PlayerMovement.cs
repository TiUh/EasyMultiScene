// Unity
using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demo
{
    public class PlayerMovement : MonoBehaviour
    {
        /// <summary>
        /// The speed that the player will move at.
        /// </summary>
        public float speed = 6f;

        /// <summary>
        /// The vector to store the direction of the player's movement.
        /// </summary>
        Vector3 movement;

        /// <summary>
        /// Reference to the animator component.
        /// </summary>
        Animator anim;

        /// <summary>
        /// Reference to the player's rigidbody.
        /// </summary>
        Rigidbody playerRigidbody;

#if !MOBILE_INPUT

        /// <summary>
        /// The length of the ray from the camera into the scene.
        /// </summary>
        float camRayLength = 100f;
#endif

        void Awake ()
        {
            // Set up references.
            this.anim = GetComponent <Animator> ();
            this.playerRigidbody = GetComponent <Rigidbody> ();
        }

        void FixedUpdate ()
        {
            // Store the input axes.
            float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            float v = CrossPlatformInputManager.GetAxisRaw("Vertical");

            // Move the player around the scene.
            this.Move (h, v);

            // Turn the player to face the mouse cursor.
            this.Turning ();

            // Animate the player.
            this.Animating (h, v);
        }

        void Move (float h, float v)
        {
            // Move only if this scene is in front.
            if (this.IsSceneActive())
            {
                // Set the movement vector based on the axis input.
                this.movement.Set(h, 0f, v);

                // Normalise the movement vector and make it proportional to the speed per second.
                this.movement = this.movement.normalized * this.speed * this.GetSceneDeltaTime();

                // Move the player to it's current position plus the movement.
                this.playerRigidbody.MovePosition(this.transform.position + this.movement);
            }
        }

        void Turning ()
        {
#if !MOBILE_INPUT
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit floorHit;

            // Perform the raycast and if it hits something on the floor layer...
            if (Physics.Raycast (camRay, out floorHit, camRayLength, this.GetSceneLayerMask()))
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = floorHit.point - this.transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation (playerToMouse);

                // Set the player's rotation to this new rotation.
                this.playerRigidbody.MoveRotation (newRotatation);
            }
#else

            Vector3 turnDir = new Vector3(CrossPlatformInputManager.GetAxisRaw("Mouse X") , 0f , CrossPlatformInputManager.GetAxisRaw("Mouse Y"));

            if (turnDir != Vector3.zero)
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = (this.transform.position + turnDir) - this.transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                this.playerRigidbody.MoveRotation(newRotatation);
            }
#endif
        }

        void Animating (float h, float v)
        {
            // Create a boolean that is true if either of the input axes is non-zero.
            bool walking = h != 0f || v != 0f;

            // Tell the animator whether or not the player is walking.
            this.anim.SetBool ("IsWalking", walking);
        }
    }
}