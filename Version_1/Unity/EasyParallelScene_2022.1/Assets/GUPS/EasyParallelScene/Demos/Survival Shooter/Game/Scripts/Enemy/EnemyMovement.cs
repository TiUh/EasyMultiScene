// Unity
using UnityEngine;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demo
{
    public class EnemyMovement : MonoBehaviour
    {
        /// <summary>
        /// Reference to the player's position.
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
        /// Reference to the nav mesh agent.
        /// </summary>
        private UnityEngine.AI.NavMeshAgent nav;

        private void Awake ()
        {
            // Set up the references.
            this.player = GameObjectHelper.FindGameObjectWithTagAndLayer("Player", this.gameObject.scene);
            this.playerHealth = player.GetComponent <PlayerHealth> ();
            this.enemyHealth = GetComponent <EnemyHealth> ();
            this.nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
        }
        
        private void Update ()
        {
            // If the enemy and the player have health left...
            if(this.enemyHealth.currentHealth > 0 && this.playerHealth.currentHealth > 0)
            {
                // Check if this ParallelScene got paused.
                if(ParallelSceneManager.Singleton.GetIsPaused(this.gameObject.scene.path))
                {
                    // ... disable the nav mesh agent. And so the movement.
                    nav.enabled = false;
                }
                else
                {
                    // ... enable the nav mesh agent. And so the movement.
                    this.nav.enabled = true;

                    // ... check if this GameObject (Enemy) is in the same ParallelScene as the player.
                    if (GameObjectHelper.GameObjectsAreInSameScene(this.gameObject, player))
                    {
                        // ... set the destination of the nav mesh agent to the player.
                        this.nav.SetDestination(this.player.transform.position);
                    } 
                    else
                    {
                        // ... set to current location. So the enemy will not move.
                        this.nav.SetDestination(this.transform.position);
                    }
                }
            }
            // Otherwise...
            else
            {
                // ... disable the nav mesh agent.
                this.nav.enabled = false;
            }
        }
    }
}