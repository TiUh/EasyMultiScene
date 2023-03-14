// Unity
using UnityEngine;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demo
{
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
        /// Reference to the animator component.
        /// </summary>
        private Animator anim;

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
            // Setting up the references.
            
            // Find the Player in the registered ParallelScene.
            this.player = GameObjectHelper.FindGameObjectWithTagAndLayer("Player", this.gameObject.scene);

            // Setup the other components.
            this.playerHealth = player.GetComponent <PlayerHealth>();
            this.enemyHealth = GetComponent<EnemyHealth>();
            this.anim = GetComponent<Animator>();
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

            // If the player has zero or less health...
            if(playerHealth.currentHealth <= 0)
            {
                // ... tell the animator the player is dead.
                anim.SetTrigger ("PlayerDead");
            }
        }

        private void Attack ()
        {
            // Check if Player and Enemy are in the same Scene.
            if (!GameObjectHelper.GameObjectsAreInSameScene(this.gameObject, this.player))
            {
                return;
            }
            
            // Reset the timer.
            timer = 0f;

            // If the player has health to lose...
            if(playerHealth.currentHealth > 0)
            {
                // ... damage the player.
                playerHealth.TakeDamage (attackDamage);
            }
        }
    }
}