// Unity
using UnityEngine;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demos.VoxelShooter
{
    /// <summary>
    /// Buy a damage upgrade for the player.
    /// </summary>
    public class BuyDamage : MonoBehaviour
    {
        /// <summary>
        /// On Player collides, buy damage if enough score is available.
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

                // Buy damage.
                if (ScoreManager.Score >= 200)
                {
                    ScoreManager.Score -= 200;
                    PlayerShooting var_PlayerShooting = _Other.GetComponentInChildren<PlayerShooting>(true);
                    var_PlayerShooting.damagePerShot += 5;
                }

                // Reset position.
                _Other.transform.position = new Vector3(3, 1, 2);
            }
        }
    }
}