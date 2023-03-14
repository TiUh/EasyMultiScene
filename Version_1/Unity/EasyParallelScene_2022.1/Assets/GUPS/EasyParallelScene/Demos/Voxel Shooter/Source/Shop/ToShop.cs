// Unity
using UnityEngine;
using UnityEngine.SceneManagement;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demos.VoxelShooter
{
    /// <summary>
    /// Teleport a Player to the "Shop" Scene on touch.
    /// If the "Shop" Scene is not loaded, it will be loaded async as parallel.
    /// </summary>
    public class ToShop : MonoBehaviour
    {
        /// <summary>
        /// The Shop Scene.
        /// </summary>
        public ParallelScene ShopScene;

        /// <summary>
        /// On Player collides with this GameObject, the shop Scene will be loaded async.
        /// While that the current scene will be paused.
        /// When the shop Scene is loaded, the Player will be teleported to the shop Scene.
        /// </summary>
        /// <param name="_Other"></param>
        private void OnTriggerEnter(Collider _Other)
        {
            // Only the Player can teleport.
            if (_Other.CompareTag("Player"))
            {
                // Check if Player and Trigger are in the same Scene.
                if (!GameObjectHelper.GameObjectsAreInSameScene(this.gameObject, _Other.gameObject))
                {
                    return;
                }
                
                // Pause this scene.
                ParallelSceneManager.Singleton.SetIsPaused(this.gameObject.scene.path, true);

                // Check if the Shop Scene is already loaded.
                if (ParallelSceneManager.Singleton.IsLoaded(this.ShopScene))
                {
                    // If the Shop Scene is already loaded, teleport the Player to the Shop Scene.
                    this.TeleportPlayerToShopScene(_Other.gameObject);
                }
                else
                {
                    // If the Shop Scene is not loaded, load the Shop Scene and teleport the Player to the Shop Scene.
                    AsyncOperation var_AsyncOperation = ParallelSceneManager.Singleton.Load(this.ShopScene, LoadSceneMode.Additive, ELoadGameObjectFlag.APPLY_LAYER);

                    // On complete loading, teleport the Player to the Shop Scene.
                    var_AsyncOperation.completed += _AsyncOperation => this.TeleportPlayerToShopScene(_Other.gameObject);
                }
            }
        }

        /// <summary>
        /// Teleport the Player to the shop Scene.
        /// </summary>
        /// <param name="_Player"></param>
        private void TeleportPlayerToShopScene(GameObject _Player)
        {
            // Move Player to the Shop Scene.
            ParallelSceneManager.Singleton.MoveGameObjectToScene(_Player, this.ShopScene);

            // Reset position.
            _Player.transform.position = new Vector3(3, 1, 2);

            // Refind Map.
            _Player.GetComponent<PlayerMovement>().FindMap();

            // At least switch to the Shop Scene.
            ParallelSceneManager.Singleton.SwitchTo(this.ShopScene, ESwitchSceneFlag.DEFAULT, true);
        }
    }
}