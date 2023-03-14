// Unity
using UnityEngine;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demos.VoxelShooter
{
    /// <summary>
    /// Simple health system for the Enemy.
    /// </summary>
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
        /// Whether the enemy is dead.
        /// </summary>
        private bool isDead;

        /// <summary>
        /// Whether the enemy has started sinking through the floor.
        /// </summary>
        private bool isSinking;

        private void Awake ()
        {
            // Setting the current health when the enemy first spawns.
            this.currentHealth = startingHealth;
        }

        private void Update ()
        {
            // If the enemy should be sinking...
            if(this.isSinking)
            {
                // ... move the enemy down by the sinkSpeed per second.
                transform.Translate (-Vector3.up * this.sinkSpeed * this.GetSceneDeltaTime());
            }
        }

        public void TakeDamage(int _Amount)
        {
            // If the enemy is dead...
            if (this.isDead)
            { 
                // ... no need to take damage so exit the function.
                return;
            }

            // Reduce the current health by the amount of damage sustained.
            this.currentHealth -= _Amount;

            // If the current health is less than or equal to zero...
            if(this.currentHealth <= 0)
            {
                // ... the enemy is dead.
                this.Death ();
            }
        }

        private void Death ()
        {
            // The enemy is dead.
            this.isDead = true;

            // Start sink and die.
            this.StartSinking();
        }

        private void StartSinking ()
        {
            // The enemy should no sink.
            this.isSinking = true;

            // Increase the score by the enemy's score value.
            ScoreManager.Score += scoreValue;

            // After 2 seconds destory the enemy.
            Destroy(gameObject, 2f);
        }
    }
}