using System;

// Unity
using UnityEngine;

namespace GUPS.EasyParallelScene
{
    /// <summary>
    /// Use these flags to control the ParallelScene GameObjects loading.
    /// </summary>
    [Flags]
    public enum ELoadGameObjectFlag : byte
    {
        /// <summary>
        /// Just load the GameObject/GameObjects.
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Load the GameObject/GameObjects and apply the ParallelScene Layer settings.
        /// Also apply the Layer to all Camera's culling and event mask and to all Light's and Reflection Probes culling mask.
        /// </summary>
        APPLY_LAYER = 1,

        /// <summary>
        /// Load the GameObject/GameObjects and merge them to the active Scene. 
        /// Combined with APPLY_LAYER, the active Scene's ParallelScene Layer settings will be applied to the loaded GameObjects.
        /// </summary>
        MERGE = 2,
    }
}