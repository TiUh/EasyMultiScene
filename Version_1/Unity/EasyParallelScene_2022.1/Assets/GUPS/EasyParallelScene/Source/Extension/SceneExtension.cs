using System;

// Unity
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GUPS.EasyParallelScene.Extension
{
    /// <summary>
    /// Extension methods for Unity Scenes, allowing faster interaction with ParallelScenes.
    /// </summary>
    public static class SceneExtension
    {
        /// <summary>
        /// Returns the registered ParallelScene for a _Scene. If there is no ParallelScene registered, will return null.
        /// </summary>
        /// <param name="_Scene">Scene to check.</param>
        /// <returns></returns>
        public static ParallelScene GetParallelScene(this Scene _Scene)
        {
            return ParallelSceneManager.Singleton.FindParallelScene(_Scene.path);
        }

        /// <summary>
        /// Returns if the _Scene is active (in front).
        /// </summary>
        /// <param name="_Scene">Scene to check.</param>
        /// <returns></returns>
        public static bool IsActive(this Scene _Scene)
        {
            return ParallelSceneManager.Singleton.IsSceneActive(_Scene);
        }

        /// <summary>
        /// Returns the layer of the ParallelScene of _Scene.
        /// Mostly used to identify GameObjects in different ParallelScenes.
        /// </summary>
        /// <param name="_Scene">Get the layer for the passed Scene.</param>
        /// <returns></returns>
        public static int GetLayer(this Scene _Scene)
        {
            return ParallelSceneManager.Singleton.GetLayer(_Scene);
        }

        /// <summary>
        /// Returns the Layer Mask of the ParallelScene of _Scene.
        /// Mostly used to cast Physics.Raycast in different ParallelScenes.
        /// </summary>
        /// <param name="_Scene">Get the layer name for the passed Scene.</param>
        /// <returns></returns>
        public static int GetLayerMask(this Scene _Scene)
        {
            // Find the Layer for the registered ParallelScene.
            int var_Layer = ParallelSceneManager.Singleton.GetLayer(_Scene);
            
            // Find the Layer name for the registered ParallelScene.
            String var_LayerName = LayerMask.LayerToName(var_Layer);

            return LayerMask.GetMask(var_LayerName);
        }
        /// <summary>
        /// Returns if the Scene is paused.
        /// </summary>
        /// <param name="_Scene">Pass a Scene you want to know if it is paused.</param>
        /// <returns>If the Scene is paused.</returns>
        public static bool GetIsPaused(this Scene _Scene)
        {
            return ParallelSceneManager.Singleton.GetIsPaused(_Scene.path);
        }

        /// <summary>
        /// Returns the current TimeScale for a ParallelScene of _Scene.
        /// </summary>
        /// <param name="_Scene">Pass a Scene to get its parallel TimeScale.</param>
        /// <returns></returns>
        public static float GetTimeScale(this Scene _Scene)
        {
            // If has the Scene registered, return its parallel TimeScale. Else use the default TimeScale.
            return ParallelSceneManager.Singleton.GetTimeScale(_Scene.path);
        }

        /// <summary>
        /// Returns the DeltaTime for a ParallelScene of _Scene.
        /// The DeltaTime is the interval in seconds from the last frame to the current one.
        /// If the ParallelScene is paused, the DeltaTime is 0.0f.
        /// </summary>
        /// <param name="_Scene">Pass a Scene to get its parallel DeltaTime.</param>
        /// <returns></returns>
        public static float GetDeltaTime(this Scene _Scene)
        {
            // If has the Scene registered, return its parallel DeltaTime. Else use the default DeltaTime.
            return ParallelSceneManager.Singleton.GetDeltaTime(_Scene.path);
        }
    }
}
