using System;

// Unity
using UnityEngine;

namespace GUPS.EasyParallelScene.Extension
{
    /// <summary>
    /// Extension methods for Unity Components, allowing faster interaction with ParallelScenes.
    /// </summary>
    public static class ComponentExtension
    {
        /// <summary>
        /// Returns if the Scene of _Component is active (in front).
        /// </summary>
        /// <param name="_Component">Pass a Component to check if the scene it is in is active.</param>
        /// <returns></returns>
        public static bool IsSceneActive(this Component _Component)
        {
            // If the scene is in front (active) returns true.
            return _Component.gameObject.scene.IsActive();
        }

        /// <summary>
        /// Returns the Layer of the ParallelScene the _Component is in.
        /// Mostly used to identify GameObjects in different ParallelScenes.
        /// </summary>
        /// <param name="_Scene">Get the layer for the passed Scene.</param>
        /// <returns></returns>
        public static int GetSceneLayer(this Component _Component)
        {
            return _Component.gameObject.scene.GetLayer();
        }

        /// <summary>
        /// Returns the Layer Mask of the ParallelScene the _Component is in.
        /// Mostly used to cast Physics.Raycast in different ParallelScenes.
        /// </summary>
        /// <param name="_Scene">Get the layer name for the passed Scene.</param>
        /// <returns></returns>
        public static int GetSceneLayerMask(this Component _Component)
        {
            return _Component.gameObject.scene.GetLayerMask();
        }

        /// <summary>
        /// Returns the DeltaTime for the Scene the _Component  is in.
        /// </summary>
        /// <param name="_Component">Pass a Component to get its parallel delta time based on the Scene it is in.</param>
        /// <returns></returns>
        public static float GetSceneDeltaTime(this Component _Component)
        {
            // If has the Scene registered, return its parallel delta time. Else use the default delta time.
            return _Component.gameObject.scene.GetDeltaTime();
        }
    }
}
