// Unity
using UnityEngine;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demo
{
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
        float timer;

        /// <summary>
        /// A ray from the gun end forwards.
        /// </summary>
        Ray shootRay = new Ray();

        /// <summary>
        /// A raycast hit to get information about what was hit.
        /// </summary>
        RaycastHit shootHit;

        /// <summary>
        /// Reference to the particle system.
        /// </summary>
        ParticleSystem gunParticles;

        /// <summary>
        /// Reference to the line renderer.
        /// </summary>
        LineRenderer gunLine;

        /// <summary>
        /// Reference to the audio source.
        /// </summary>
        AudioSource gunAudio;

        /// <summary>
        /// Reference to the light component.
        /// </summary>
        Light gunLight;

        /// <summary>
        /// Reference to the light component to the face.
        /// </summary>
		public Light faceLight;

        /// <summary>
        /// The proportion of the timeBetweenBullets that the effects will display for.
        /// </summary>
        float effectsDisplayTime = 0.2f;

        void Awake ()
        {
            // Set up the references.
            this.gunParticles = GetComponent<ParticleSystem> ();
            this.gunLine = GetComponent <LineRenderer> ();
            this.gunAudio = GetComponent<AudioSource> ();
            this.gunLight = GetComponent<Light> ();
        }

        void Update ()
        {
            // Add the time since Update was last called to the timer.
            this.timer += this.GetSceneDeltaTime();

            // Shoot only if this scene is in front.
            if (this.IsSceneActive())
            {
#if !MOBILE_INPUT
                // If the Fire1 button is being press and it's time to fire...
                if (Input.GetButton("Fire1") && this.timer >= this.timeBetweenBullets && this.gameObject.scene.GetTimeScale() != 0)
                {
                    // ... shoot the gun.
                    this.Shoot();
                }
#else
                // If there is input on the shoot direction stick and it's time to fire...
                if ((CrossPlatformInputManager.GetAxisRaw("Mouse X") != 0 || CrossPlatformInputManager.GetAxisRaw("Mouse Y") != 0) && this.timer >= this.timeBetweenBullets)
                {
                    // ... shoot the gun
                    this.Shoot();
                }
#endif
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
            this.faceLight.enabled = false;
            this.gunLight.enabled = false;
        }

        void Shoot ()
        {
            // Reset the timer.
            this.timer = 0f;

            // Play the gun shot audioclip.
            this.gunAudio.Play ();

            // Enable the lights.
            this.gunLight.enabled = true;
            this.faceLight.enabled = true;

            // Stop the particles from playing if they were, then start the particles.
            this.gunParticles.Stop ();
            this.gunParticles.Play ();

            // Enable the line renderer and set it's first position to be the end of the gun.
            this.gunLine.enabled = true;
            this.gunLine.SetPosition (0, transform.position);

            // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
            this.shootRay.origin = transform.position;
            this.shootRay.direction = transform.forward;

            // Perform the raycast against gameobjects on the shootable layer and if it hits something...
            if (Physics.Raycast (shootRay, out shootHit, this.range, this.GetSceneLayerMask()))
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();

                // If the EnemyHealth component exist...
                if(enemyHealth != null)
                {
                    // ... the enemy should take damage.
                    enemyHealth.TakeDamage (this.damagePerShot, shootHit.point);
                }

                // Set the second position of the line renderer to the point the raycast hit.
                this.gunLine.SetPosition (1, shootHit.point);
            }
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                this.gunLine.SetPosition (1, shootRay.origin + shootRay.direction * this.range);
            }
        }
    }
}