// Unity
using UnityEngine;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demo
{
    /// <summary>
    /// Buy healing for the player.
    /// </summary>
    public class BuyHealing : MonoBehaviour
    {
        /// <summary>
        /// On Player collides, buy healing if enough score is available.
        /// </summary>
        /// <param name="_Other"></param>
        private void OnTriggerEnter(Collider _Other)
        {
            // Only the Player can buy.
            if (_Other.CompareTag("Player"))
            {
                // Check if Player and Trigger are in the same Scene.
                if (!GameObjectHelper.GameObjectsAreInSameScene(this.gameObject, _Other.gameObject))
                {
                    return;
                }

                // Buy healing.
                if (ScoreManager.Score >= 100)
                {
                    ScoreManager.Score -= 100;
                    PlayerHealth playerHealth = _Other.GetComponent<PlayerHealth>();
                    playerHealth.Heal(25);
                }

                // Reset position.
                _Other.transform.position = Vector3.zero;
            }
        }
    }
}