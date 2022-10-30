using System;

// Unity
using UnityEngine;

namespace GUPS.EasyParallelScene
{
    public enum ELoadParallelSceneMode : byte
    {
        /// <summary>
        /// Load the scene parallel without merging the GameObjects nor applying any layer settings to them.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Loads the scene an merge it to the current loaded
        /// </summary>
        Merge,

    }
}