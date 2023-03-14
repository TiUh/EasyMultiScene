using UnityEngine;

namespace GUPS.EasyParallelScene.Demo
{
    public class GameOverManager : MonoBehaviour
    {
        /// <summary>
        /// Reference to the player's health.
        /// </summary>
        public PlayerHealth playerHealth;

        /// <summary>
        /// Reference to the animator component.
        /// </summary>
        Animator anim;

        private void Awake ()
        {
            // Set up the reference.
            this.anim = GetComponent <Animator> ();
        }

        private void Update ()
        {
            // If the player has run out of health...
            if(this.playerHealth.currentHealth <= 0)
            {
                // ... tell the animator the game is over.
                this.anim.SetTrigger ("GameOver");
            }
        }
    }
}