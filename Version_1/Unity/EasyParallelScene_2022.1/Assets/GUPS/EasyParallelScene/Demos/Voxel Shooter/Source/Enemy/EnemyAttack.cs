// Unity
using UnityEngine;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demos.VoxelShooter
{
    /// <summary>
    /// Attacks the Player in a certain interval, if the Enemy is near enough and in the same ParallelScene.
    /// The Enemy does not attack if the ParallelScene is paused. Also the interval depends on the ParallelScene set DeltaTime.
    /// </summary>
    public class EnemyAttack : MonoBehaviour
    {
        /// <summary>
        /// The time in seconds between each attack.
        /// </summary>
        public float timeBetweenAttacks = 0.5f;

        /// <summary>
        /// The amount of health taken away per attack.
        /// </summary>
        public int attackDamage = 10;

        /// <summary>
        /// Reference to the player GameObject.
        /// </summary>
        private GameObject player;

        /// <summary>
        /// Reference to the player's health.
        /// </summary>
        private PlayerHealth playerHealth;

        /// <summary>
        /// Reference to this enemy's health.
        /// </summary>
        private EnemyHealth enemyHealth;

        /// <summary>
        /// Whether player is within the trigger collider and can be attacked.
        /// </summary>
        private bool playerInRange;

        /// <summary>
        /// Timer for counting up to the next attack.
        /// </summary>
        private float timer;

        private void Awake ()
        {
            // Find the Player in the same ParallelScene the Enemy is in, based on the Tag "Player" and ParallelScene Layer.
            this.player = GameObjectHelper.FindGameObjectWithTagAndLayer("Player", this.gameObject.scene);

            // Setup the other components.
            this.playerHealth = player.GetComponent <PlayerHealth>();
            this.enemyHealth = GetComponent<EnemyHealth>();
        }
        
        private void OnTriggerEnter (Collider other)
        {
            // If the entering collider is the player...
            if(other.gameObject == player)
            {
                // ... the player is in range.
                playerInRange = true;
            }
        }

        private void OnTriggerExit (Collider other)
        {
            // If the exiting collider is the player...
            if(other.gameObject == player)
            {
                // ... the player is no longer in range.
                playerInRange = false;
            }
        }

        private void Update ()
        {
            // Add the time since Update was last called to the timer. Based on the settings of the ParallelScene.
            timer += this.GetSceneDeltaTime();

            // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
            if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
            {
                // ... attack.
                Attack ();
            }
        }

        private void Attack ()
        {
            // Check if the Player and Enemy are in the same ParallelScene.
            if (!GameObjectHelper.GameObjectsAreInSameScene(this.gameObject, this.player))
            {
                return;
            }
            
            // Check if this ParallelScene is not paused.
            if (!ParallelSceneManager.Singleton.GetIsPaused(this.gameObject.scene.path))
            {
                // Reset the timer.
                timer = 0f;

                // If the player has health to lose...
                if (playerHealth.currentHealth > 0)
                {
                    // ... damage the player.
                    playerHealth.TakeDamage(attackDamage);
                }
            }
        }
    }
}