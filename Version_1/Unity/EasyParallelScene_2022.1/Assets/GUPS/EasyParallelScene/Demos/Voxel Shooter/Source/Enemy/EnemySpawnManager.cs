// Unity
using UnityEngine;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demos.VoxelShooter
{
    /// <summary>
    /// Spawns the Enemies at SpawnPoints.
    /// </summary>
    public class EnemySpawnManager : MonoBehaviour
    {
        /// <summary>
        /// Reference to the player's heatlh.
        /// </summary>
        public PlayerHealth playerHealth;

        /// <summary>
        /// The enemy prefab to be spawned.
        /// </summary>
        public GameObject enemy;

        /// <summary>
        /// How long between each spawn.
        /// </summary>
        public float spawnTime = 3f;

        /// <summary>
        /// An array of the spawn points this enemy can spawn from.
        /// </summary>
        public Transform[] spawnPoints;

        private void Start ()
        {
            // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
            InvokeRepeating ("Spawn", spawnTime, spawnTime);
        }

        private void Spawn ()
        {
            // If the Scene is paused, do not spawn new Enemies. EasyParallelScene provides different ways to check if a Scene is paused through helper and extensions.
            if (this.gameObject.scene.GetIsPaused())
            {
                return;
            }

            // If the player has no health left...
            if (this.playerHealth.currentHealth <= 0f)
            {
                // ... exit the function.
                return;
            }

            // Find a random index between zero and one less than the number of spawn points.
            int spawnPointIndex = Random.Range (0, this.spawnPoints.Length);

            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation and spawn it in the ParallelScene of this manager.
            ParallelSceneManager.Singleton.Instantiate(this.enemy, this.spawnPoints[spawnPointIndex].position, this.spawnPoints[spawnPointIndex].rotation, this.gameObject.scene.GetParallelScene());
        }
    }
}