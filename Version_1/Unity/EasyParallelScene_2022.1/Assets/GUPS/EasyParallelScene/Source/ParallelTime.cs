namespace GUPS.EasyParallelScene
{
    /// <summary>
    /// A structure storing the runtime time scale and pausing of an ParallelScene.
    /// </summary>
    internal struct ParallelTime
    {
        /// <summary>
        /// Get the time scale.
        /// </summary>
        public float TimeScale { get; }

        /// <summary>
        /// Get if is paused.
        /// </summary>
        public bool IsPaused { get; }

        /// <summary>
        /// Create a new ParallelTime structure.
        /// </summary>
        /// <param name="_TimeScale"></param>
        /// <param name="_IsPaused"></param>
        public ParallelTime(float _TimeScale, bool _IsPaused)
        {
            this.TimeScale = _TimeScale;
            this.IsPaused = _IsPaused;
        }
    }
}
