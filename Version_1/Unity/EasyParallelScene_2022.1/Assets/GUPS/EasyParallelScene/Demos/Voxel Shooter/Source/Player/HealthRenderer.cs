// Unity
using UnityEngine;
using UnityEngine.UI;

namespace GUPS.EasyParallelScene.Demos.VoxelShooter
{
    /// <summary>
    /// Renderer for the Player health in the UI as Slider.
    /// </summary>
    public class HealthRenderer : MonoBehaviour
    {
        /// <summary>
        /// Reference to the Slider to display the Player's health.
        /// </summary>
        public Slider healthSlider;

        /// <summary>
        /// The PlayerHealth component on the Player.
        /// </summary>
        private PlayerHealth playerHealth;

        private void Awake()
        {
            // Find the Player by its Tag.
            GameObject var_Player = GameObject.FindGameObjectWithTag("Player");

            // Find the PlayerHealth component in the Player.
            this.playerHealth = var_Player.GetComponent<PlayerHealth>();
        }

        private void Update()
        {
            // Set the health bar's value to the current Player health.
            this.healthSlider.value = this.playerHealth.currentHealth;
        }
    }
}