// Unity
using UnityEngine;

namespace GUPS.EasyParallelScene.Demos.VoxelShooter
{
    /// <summary>
    /// Simple Singleton Manager to keep track of the Player Score.
    /// </summary>
    public class ScoreManager : GUPS.EasyParallelScene.Singleton.PersistentSingleton<ScoreManager>
    {
        /// <summary>
        /// The Score.
        /// </summary>
        public static int Score { get; set; }
    }
}