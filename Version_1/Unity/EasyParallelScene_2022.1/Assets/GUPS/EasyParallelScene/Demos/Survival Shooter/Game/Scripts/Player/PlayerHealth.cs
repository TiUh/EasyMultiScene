// Unity
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demo
{
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
        /// Reference to the slider to display the player's health.
        /// </summary>
        public Slider healthSlider;

        /// <summary>
        /// Reference to the image to flash on the screen on being hurt.
        /// </summary>
        public Image damageImage;

        /// <summary>
        /// The audio clip to play when the player dies.
        /// </summary>
        public AudioClip deathClip;

        /// <summary>
        /// The speed the damageImage will fade at.
        /// </summary>
        public float flashSpeed = 5f;

        /// <summary>
        /// The colour the damageImage is set to, to flash.
        /// </summary>
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

        /// <summary>
        /// Reference to the Animator component.
        /// </summary>
        Animator anim;

        /// <summary>
        /// Reference to the player's movement.
        /// </summary>
        PlayerMovement playerMovement;

        /// <summary>
        /// Reference to the PlayerShooting script.
        /// </summary>
        PlayerShooting playerShooting;

        /// <summary>
        /// Whether the player is dead.
        /// </summary>
        bool isDead;

        /// <summary>
        /// True when the player gets damaged.
        /// </summary>
        bool damaged;
        
        void Awake ()
        {
            // Setting up the references.
            this.anim = GetComponent <Animator> ();
            this.playerMovement = GetComponent <PlayerMovement> ();
            this.playerShooting = GetComponentInChildren <PlayerShooting> ();

            // Set the initial health of the player.
            this.currentHealth = startingHealth;
        }

        void Update ()
        {
            // If the player has just been damaged...
            if(this.damaged)
            {
                // ... set the colour of the damageImage to the flash colour.
                this.damageImage.color = flashColour;
            }
            // Otherwise...
            else
            {
                // ... transition the colour back to clear.
                this.damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * this.GetSceneDeltaTime());
            }

            // Reset the damaged flag.
            this.damaged = false;
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

            // Set the health bar's value to the current health.
            this.healthSlider.value = this.currentHealth;
        }

        public void TakeDamage (int amount)
        {
            // Set the damaged flag so the screen will flash.
            this.damaged = true;

            // Reduce the current health by the damage amount.
            this.currentHealth -= amount;

            // Set the health bar's value to the current health.
            this.healthSlider.value = this.currentHealth;
            
            // If the player has lost all it's health and the death flag hasn't been set yet...
            if(this.currentHealth <= 0 && !this.isDead)
            {
                // ... it should die.
                this.Death ();
            }
        }
        
        void Death ()
        {
            // Set the death flag so this function won't be called again.
            this.isDead = true;

            // Turn off any remaining shooting effects.
            this.playerShooting.DisableEffects ();

            // Tell the animator that the player is dead.
            this.anim.SetTrigger ("Die");

            // Turn off the movement and shooting scripts.
            this.playerMovement.enabled = false;
            this.playerShooting.enabled = false;
        }


        public void RestartLevel ()
        {
            // Reset ScoreManager.
            ScoreManager.Score = 0;

            // Reload the level that is currently loaded.
            ParallelSceneManager.Singleton.Load(ParallelSceneManager.Singleton.FindParallelScene("Game"), LoadSceneMode.Single, ELoadGameObjectFlag.APPLY_LAYER);
        }
    }
}