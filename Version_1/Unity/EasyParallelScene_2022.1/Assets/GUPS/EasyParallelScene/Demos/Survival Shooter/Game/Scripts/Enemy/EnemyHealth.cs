// Unity
using UnityEngine;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demo
{
    public class EnemyHealth : MonoBehaviour
    {
        /// <summary>
        /// The amount of health the enemy starts the game with.
        /// </summary>
        public int startingHealth = 100;

        /// <summary>
        /// The current health the enemy has.
        /// </summary>
        public int currentHealth;

        /// <summary>
        /// The speed at which the enemy sinks through the floor when dead.
        /// </summary>
        public float sinkSpeed = 2.5f;

        /// <summary>
        /// The amount added to the player's score when the enemy dies.
        /// </summary>
        public int scoreValue = 10;

        /// <summary>
        /// The sound to play when the enemy dies.
        /// </summary>
        public AudioClip deathClip;

        /// <summary>
        /// Reference to the animator.
        /// </summary>
        private Animator anim;

        /// <summary>
        /// Reference to the audio source.
        /// </summary>
        private AudioSource enemyAudio;

        /// <summary>
        /// Reference to the particle system that plays when the enemy is damaged.
        /// </summary>
        private ParticleSystem hitParticles;

        /// <summary>
        /// Reference to the capsule collider.
        /// </summary>
        private CapsuleCollider capsuleCollider;

        /// <summary>
        /// Whether the enemy is dead.
        /// </summary>
        private bool isDead;

        /// <summary>
        /// Whether the enemy has started sinking through the floor.
        /// </summary>
        private bool isSinking;

        private void Awake ()
        {
            // Setting up the references.
            anim = GetComponent <Animator> ();
            enemyAudio = GetComponent <AudioSource> ();
            hitParticles = GetComponentInChildren <ParticleSystem> ();
            capsuleCollider = GetComponent <CapsuleCollider> ();

            // Setting the current health when the enemy first spawns.
            currentHealth = startingHealth;
        }

        private void Update ()
        {
            // If the enemy should be sinking...
            if(isSinking)
            {
                // ... move the enemy down by the sinkSpeed per second.
                transform.Translate (-Vector3.up * sinkSpeed * this.GetSceneDeltaTime());
            }
        }

        public void TakeDamage (int amount, Vector3 hitPoint)
        {
            // If the enemy is dead...
            if(isDead)
                // ... no need to take damage so exit the function.
                return;

            // Play the hurt sound effect.
            enemyAudio.Play ();

            // Reduce the current health by the amount of damage sustained.
            currentHealth -= amount;
            
            // Set the position of the particle system to where the hit was sustained.
            hitParticles.transform.position = hitPoint;

            // And play the particles.
            hitParticles.Play();

            // If the current health is less than or equal to zero...
            if(currentHealth <= 0)
            {
                // ... the enemy is dead.
                Death ();
            }
        }

        void Death ()
        {
            // The enemy is dead.
            isDead = true;

            // Turn the collider into a trigger so shots can pass through it.
            capsuleCollider.isTrigger = true;

            // Tell the animator that the enemy is dead.
            anim.SetTrigger ("Dead");

            // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
            enemyAudio.clip = deathClip;
            enemyAudio.Play ();
        }

        public void StartSinking ()
        {
            // Find and disable the Nav Mesh Agent.
            GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;

            // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
            GetComponent <Rigidbody> ().isKinematic = true;

            // The enemy should no sink.
            isSinking = true;

            // Increase the score by the enemy's score value.
            ScoreManager.Score += scoreValue;

            // After 2 seconds destory the enemy.
            Destroy (gameObject, 2f);
        }
    }
}