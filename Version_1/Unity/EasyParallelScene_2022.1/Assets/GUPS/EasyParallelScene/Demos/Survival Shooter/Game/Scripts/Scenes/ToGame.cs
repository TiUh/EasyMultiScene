// Unity
using UnityEngine;
using UnityEngine.SceneManagement;

// GUPS
using GUPS.EasyParallelScene;
using GUPS.EasyParallelScene.Extension;
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Demo
{
    /// <summary>
    /// Teleport a Player to the game Scene on contact.
    /// </summary>
    public class ToGame : MonoBehaviour
    {
        /// <summary>
        /// The Game Scene.
        /// </summary>
        public ParallelScene GameScene;

        /// <summary>
        /// On Player collides with this GameObject, the Game Scene will be loaded async.
        /// While that the current scene will be paused.
        /// When the Game Scene is loaded, the Player will be teleported to the Game Scene.
        /// </summary>
        /// <param name="_Other"></param>
        private void OnTriggerEnter(Collider _Other)
        {
            // Only the Player can teleport.
            if (_Other.CompareTag("Player"))
            {
                // Check if Player and Trigger are in the same Scene.
                if(!GameObjectHelper.GameObjectsAreInSameScene(this.gameObject, _Other.gameObject))
                {
                    return;
                }

                // Pause this scene.
                ParallelSceneManager.Singleton.SetIsPaused(this.gameObject.scene.path, true);

                // Check if the Game Scene is already loaded.
                if (ParallelSceneManager.Singleton.IsLoaded(this.GameScene))
                {
                    // If the Game Scene is already loaded, teleport the Player to the Game Scene.
                    this.TeleportPlayerToGameScene(_Other.gameObject);
                }
                else
                {
                    // If the Game Scene is not loaded, load the Game Scene and teleport the Player to the Game Scene.
                    AsyncOperation var_AsyncOperation = ParallelSceneManager.Singleton.Load(this.GameScene, LoadSceneMode.Additive, ELoadGameObjectFlag.APPLY_LAYER);

                    // On complete loading, teleport the Player to the Game Scene.
                    var_AsyncOperation.completed += _AsyncOperation => this.TeleportPlayerToGameScene(_Other.gameObject);
                }
            }
        }

        /// <summary>
        /// Teleport the Player to the Game Scene.
        /// </summary>
        /// <param name="_Player"></param>
        private void TeleportPlayerToGameScene(GameObject _Player)
        {
            // Move Player to the Game Scene.
            ParallelSceneManager.Singleton.MoveGameObjectToScene(_Player, this.GameScene);

            // Reset position.
            _Player.transform.position = Vector3.zero;

            // At least switch to the Game Scene.
            ParallelSceneManager.Singleton.SwitchTo(this.GameScene, ESwitchSceneFlag.DEFAULT, true);
        }
    }
}