using System;

// Unity
using UnityEngine;

namespace GUPS.EasyParallelScene
{
    /// <summary>
    /// A parallel scene wraps around a unity scene and makes it parallelizable.
    /// To create a parallel scene, just right click anywhere in your project view and click Create->GUPS->EasyParallelScene->ParallelScene.
    /// </summary>
    [CreateAssetMenu(fileName = "NewParallelScene", menuName = "GUPS/EasyParallelScene/ParallelScene", order = Int32.MaxValue)]
    public class ParallelScene : ScriptableObject
    {
        /// <summary>
        /// The path of the scene.
        /// Allowed: A relative scene path, like 'Assets/.../MyScene.unity'.
        /// </summary>
        [SerializeField]
        private String scenePath;

        /// <summary>
        /// The path of the scene.
        /// Allowed: A relative scene path, like 'Assets/.../MyScene.unity'.
        /// </summary>
        public String ScenePath { get { return this.scenePath; } }

        /// <summary>
        /// The layer the scene uses.
        /// Allowed: -1 as 'null' and 0 to 31 as unity layer.
        /// </summary>
        [SerializeField]
        private int layer = 0;

        /// <summary>
        /// The layer the scene uses.
        /// Allowed: -1 as 'null' and 0 to 31 as unity layer.
        /// </summary>
        public int Layer { get { return this.layer; } }

        /// <summary>
        /// Custom time scale the scene uses.
        /// </summary>
        [SerializeField]
        private float timeScale = 1.0f;

        /// <summary>
        /// Custom time scale the scene uses.
        /// </summary>
        public float TimeScale { get { return this.timeScale; } }

        /// <summary>
        /// Calculates a hash based on the class fields. The hash is useful for multiplayer to ensure the parallel scene settings are all the same.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int var_Hash = 73;
            var_Hash ^= (this.scenePath != null ? this.scenePath.GetHashCode() : 83) * 27;
            var_Hash ^= this.layer.GetHashCode() * 27;
            var_Hash ^= this.timeScale.GetHashCode() * 27;

            return var_Hash;
        }

        /// <summary>
        /// Represents the parallel scene as string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "ParallelScene [scenePath = " + this.scenePath + ", layer = " + this.layer + ", timeScale = " + this.timeScale + "]";
        }
    }
}