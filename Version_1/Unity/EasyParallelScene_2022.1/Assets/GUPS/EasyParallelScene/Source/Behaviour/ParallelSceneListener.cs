using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Unity
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GUPS.EasyParallelScene.Behaviour
{
    /// <summary>
    /// Attach this MonoBehaviour to your GameObject to listen and so react to Scene switching.
    /// </summary>
    public class ParallelSceneListener : MonoBehaviour
    {
        /// <summary>
        /// Stores if the Scene is currently in front (active) or back (parallel).
        /// </summary>
        private bool isInActiveScene;

        /// <summary>
        /// Register to the SceneManager.activeSceneChanged event.
        /// </summary>
        public virtual void Awake()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        /// <summary>
        /// Unity callback, called when the active Scene is changed.
        /// </summary>
        /// <param name="_Current"></param>
        /// <param name="_Next"></param>
        private void OnActiveSceneChanged(Scene _Current, Scene _Next)
        {
            // Check if the GameObjects Scene is active or not.
            bool var_NextSceneActive = this.gameObject.scene == _Next;

            if (this.isInActiveScene && !var_NextSceneActive)
            {
                // Assign status.
                this.isInActiveScene = var_NextSceneActive;

                // Is no longer in the active Scene.
                SendMessage("OnSceneInParallel");
            }
            else if (!this.isInActiveScene && var_NextSceneActive)
            {
                // Assign status.
                this.isInActiveScene = var_NextSceneActive;

                // Is now in the active Scene.
                SendMessage("OnSceneInFront");
            }
        }

        /// <summary>
        /// On Scene is no longer the active Scene and now in the back in parallel.
        /// </summary>
        protected virtual void OnSceneInParallel()
        {
        }

        /// <summary>
        /// On Scene is now the active Scene and so in front.
        /// </summary>
        protected virtual void OnSceneInFront()
        {
        }

        /// <summary>
        /// Unregister from the SceneManager.activeSceneChanged event.
        /// </summary>
        public virtual void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }
    }
}
