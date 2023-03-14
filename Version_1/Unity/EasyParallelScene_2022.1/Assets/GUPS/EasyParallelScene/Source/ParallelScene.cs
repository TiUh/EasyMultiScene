using System;
using System.Reflection;

// Unity
using UnityEngine;

namespace GUPS.EasyParallelScene
{
    /// <summary>
    /// A ParallelScene wraps around a Unity Scene and makes it parallelizable.
    /// This allows unique rendering, collision and light.
    /// To create a ParallelScene, just right click anywhere in your project view and click Create->GUPS->EasyParallelScene->ParallelScene.
    /// Assign to this ParallelScene a Scene and apply your custom settings.
    /// </summary>
    [ObfuscationAttribute(Exclude=true)]
    [CreateAssetMenu(fileName = "NewParallelScene", menuName = "GUPS/EasyParallelScene/ParallelScene", order = Int32.MaxValue)]
    public class ParallelScene : ScriptableObject
    {
        /// <summary>
        /// The relative path of a Unity Scene.
        /// Allowed: A relative Unity Scene path, like 'Assets/.../MyScene.unity'.
        /// </summary>
        [SerializeField]
        private String scenePath;

        /// <summary>
        /// The relative path of a Unity Scene.
        /// Allowed: A relative Unity Scene path, like 'Assets/.../MyScene.unity'.
        /// </summary>
        public String ScenePath { get { return this.scenePath; } }

        /// <summary>
        /// The layer the Scene uses. Used for rendering, collision and light.
        /// Allowed: 0 to 31 from Unity layer.
        /// </summary>
        [SerializeField]
        private int layer = 0;

        /// <summary>
        /// The layer the Scene uses. Used for rendering, collision and light.
        /// Allowed: 0 to 31 from Unity layer.
        /// </summary>
        public int Layer { get { return this.layer; } }

        /// <summary>
        /// Use a custom time scale for this Scene.
        /// </summary>
        [SerializeField]
        private bool useCustomTimeScale = false;

        /// <summary>
        /// Use a custom time scale for this Scene.
        /// </summary>
        public bool UseCustomTimeScale { get { return this.useCustomTimeScale; } }

        /// <summary>
        /// Custom time scale the Scene uses.
        /// </summary>
        [SerializeField]
        private float timeScale = 1.0f;

        /// <summary>
        /// Returns the custom time scale for the Scene, if "UseCustomTimeScale" is true.
        /// Else returns the Unity Time.TimeScale.
        /// </summary>
        public float TimeScale { get { return this.useCustomTimeScale ? this.timeScale : Time.timeScale; } }

        /// <summary>
        /// Calculates a hash based on the class fields. The hash is useful for multiplayer to ensure the ParallelScene settings are all the same.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int var_Hash = 0;
            var_Hash ^= 73 + (this.scenePath != null ? this.scenePath.GetHashCode() : 0) * 27;
            var_Hash ^= 73 + this.layer.GetHashCode() * 27;
            var_Hash ^= 73 + this.useCustomTimeScale.GetHashCode() * 27;
            var_Hash ^= 73 + this.timeScale.GetHashCode() * 27;

            return var_Hash;
        }

        /// <summary>
        /// Represents the ParallelScene as string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "ParallelScene [scenePath = " + this.scenePath + ", layer = " + this.layer + ", useCustomTimeScale = " + this.useCustomTimeScale + ", timeScale = " + this.timeScale + "]";
        }
    }
}