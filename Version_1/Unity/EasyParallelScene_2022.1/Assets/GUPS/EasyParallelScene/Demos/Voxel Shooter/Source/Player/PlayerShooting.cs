// Unity
using UnityEngine;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demos.VoxelShooter
{
    /// <summary>
    /// This class is used to shoot bullets.
    /// </summary>
    public class PlayerShooting : MonoBehaviour
    {
        /// <summary>
        /// The damage inflicted by each bullet.
        /// </summary>
        public int damagePerShot = 20;

        /// <summary>
        /// The time between each shot.
        /// </summary>
        public float timeBetweenBullets = 0.15f;

        /// <summary>
        /// The distance the gun can fire.
        /// </summary>
        public float range = 100f;

        /// <summary>
        /// A timer to determine when to fire.
        /// </summary>
        private float timer;

        /// <summary>
        /// A ray from the gun end forwards.
        /// </summary>
        private Ray shootRay = new Ray();

        /// <summary>
        /// A raycast hit to get information about what was hit.
        /// </summary>
        private RaycastHit shootHit;

        /// <summary>
        /// Reference to the line renderer.
        /// </summary>
        private LineRenderer gunLine;

        /// <summary>
        /// The proportion of the timeBetweenBullets that the effects will display for.
        /// </summary>
        private float effectsDisplayTime = 0.2f;

        private void Awake ()
        {
            // Set up the references.
            this.gunLine = this.GetComponent <LineRenderer> ();
        }

        private void Update ()
        {
            // Add the time since Update was last called to the timer.
            this.timer += this.GetSceneDeltaTime();

            // Shoot only if this scene is in front.
            if (this.IsSceneActive())
            {
                // If the Fire1 button is being press and it's time to fire...
                if (Input.GetButton("Fire1") && this.timer >= this.timeBetweenBullets && this.gameObject.scene.GetTimeScale() != 0)
                {
                    // ... shoot the gun.
                    this.Shoot();
                }
            }
            
            // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
            if (this.timer >= this.timeBetweenBullets * this.effectsDisplayTime)
            {
                // ... disable the effects.
                this.DisableEffects ();
            }
        }

        public void DisableEffects ()
        {
            // Disable the line renderer and the light.
            this.gunLine.enabled = false;
        }

        private void Shoot ()
        {
            // Reset the timer.
            this.timer = 0f;

            // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
            this.shootRay.origin = transform.position + Vector3.up / 2;
            this.shootRay.direction = new Vector3(transform.forward.x, 0, transform.forward.z);

            // Enable the line renderer and set it's first position to be the end of the gun.
            this.gunLine.enabled = true;
            this.gunLine.SetPosition (0, this.shootRay.origin);

            // Perform the raycast against gameobjects on the shootable layer and if it hits something...
            if (Physics.Raycast (this.shootRay, out shootHit, this.range, this.GetSceneLayerMask()))
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();

                // If the EnemyHealth component exist...
                if(enemyHealth != null)
                {
                    // ... the enemy should take damage.
                    enemyHealth.TakeDamage (this.damagePerShot);
                }
                // Set the second position of the line renderer to the point the raycast hit.
                this.gunLine.SetPosition(1, shootHit.point);
            }
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                this.gunLine.SetPosition(1, this.shootRay.origin + this.shootRay.direction * this.range);
            }
        }
    }
}