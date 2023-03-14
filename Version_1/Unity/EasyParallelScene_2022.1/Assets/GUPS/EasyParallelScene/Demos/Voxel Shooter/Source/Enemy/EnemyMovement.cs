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
    /// The Enemy moves towards the Player, if they are in the same ParallelScene and if the ParallelScene is not paused.
    /// </summary>
    public class EnemyMovement : MonoBehaviour
    {
        /// <summary>
        /// Reference to the world Map.
        /// </summary>
        private Map map;

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
        /// The Enemy moving speed.
        /// </summary>
        public float Speed = 3f;

        private void Awake ()
        {
            // Set up the references.
            this.map = GameObjectHelper.FindGameObjectsWithLayer(this.gameObject.scene).Where(g => g.name == "Map").FirstOrDefault()?.GetComponent<Map>();
            this.player = GameObjectHelper.FindGameObjectWithTagAndLayer("Player", this.gameObject.scene);
            this.playerHealth = this.player.GetComponent<PlayerHealth>();
            this.enemyHealth = this.GetComponent<EnemyHealth>();
        }
        
        private void Update ()
        {
            // If the enemy and the player have health left...
            if(this.enemyHealth.currentHealth > 0 && this.playerHealth.currentHealth > 0)
            {
                // Check if this ParallelScene is not paused.
                if (!ParallelSceneManager.Singleton.GetIsPaused(this.gameObject.scene.path))
                {
                    // ... check if this GameObject (Enemy) is in the same ParallelScene as the Player.
                    if (GameObjectHelper.GameObjectsAreInSameScene(this.gameObject, player))
                    {
                        // Direction to the Player.
                        Vector3 var_Direction = this.player.transform.position - this.transform.position;

                        if (var_Direction.magnitude > 1f)
                        { 
                            var_Direction = new Vector3(var_Direction.x, 0, var_Direction.z);

                            // Move in Player direction.
                            Vector3 var_Movement = var_Direction.normalized * this.Speed * this.GetSceneDeltaTime();

                            Vector3 var_TargetPosition = this.transform.position + var_Movement;

                            // Check if Block in front is empty and there is a block in front below.
                            if (this.map.GetBlock(var_TargetPosition + Vector3.up / 2f + var_Movement.normalized / 2) == null)
                            {
                                this.transform.position = var_TargetPosition;
                            }
                        }
                    }
                }
            }
        }
    }
}