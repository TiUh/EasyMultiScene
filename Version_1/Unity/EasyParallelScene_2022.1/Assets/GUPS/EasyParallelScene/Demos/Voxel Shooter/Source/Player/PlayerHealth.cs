// Unity
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demos.VoxelShooter
{
    /// <summary>
    /// This class is used to manage the Player's health.
    /// If the Player dies, the "Game" is reloaded.
    /// </summary>
    public class PlayerHealth : MonoBehaviour
    {
        /// <summary>
        /// The amount of health the player starts the game with.
        /// </summary>
        public int startingHealth = 100;

        /// <summary>
        /// The current health the player has.
        /// </summary>
        public int currentHealth;
        
        /// <summary>
        /// Reference to the player's movement.
        /// </summary>
        private PlayerMovement playerMovement;

        /// <summary>
        /// Reference to the PlayerShooting script.
        /// </summary>
        private PlayerShooting playerShooting;

        /// <summary>
        /// Whether the player is dead.
        /// </summary>
        private bool isDead;

        private void Awake ()
        {
            // Setting up the references.
            this.playerMovement = GetComponent <PlayerMovement> ();
            this.playerShooting = GetComponentInChildren <PlayerShooting> ();

            // Set the initial health of the player.
            this.currentHealth = startingHealth;
        }

        public void Heal(int _Amount)
        {
            // Add the amount to the player's current health.
            this.currentHealth += _Amount;

            // If the current health is greater than the maximum health...
            if (this.currentHealth > this.startingHealth)
            {
                // ... set the current health to the maximum health.
                this.currentHealth = this.startingHealth;
            }
        }

        public void TakeDamage (int amount)
        {
            // Reduce the current health by the damage amount.
            this.currentHealth -= amount;
            
            // If the player has lost all it's health and the death flag hasn't been set yet...
            if(this.currentHealth <= 0 && !this.isDead)
            {
                // ... it should die.
                this.Death ();
            }
        }

        private void Death ()
        {
            // Set the death flag so this function won't be called again.
            this.isDead = true;

            // Turn off any remaining shooting effects.
            this.playerShooting.DisableEffects ();

            // Turn off the movement and shooting scripts.
            this.playerMovement.enabled = false;
            this.playerShooting.enabled = false;

            // Reload the Game.
            this.RestartLevel();
        }

        private void RestartLevel ()
        {
            // Reset ScoreManager.
            ScoreManager.Score = 0;

            // Find the ParallelScene "Game" and load it as "Single". So each other Scene will be unloaded and the "Game" Scene new loaded.
            ParallelScene var_GameScene = ParallelSceneManager.Singleton.FindParallelScene("Game");
            ParallelSceneManager.Singleton.Load(var_GameScene, LoadSceneMode.Single, ELoadGameObjectFlag.APPLY_LAYER);
        }
    }
}