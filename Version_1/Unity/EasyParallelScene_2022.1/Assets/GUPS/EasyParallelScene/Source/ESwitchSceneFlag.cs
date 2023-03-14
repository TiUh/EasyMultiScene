using System;

// Unity
using UnityEngine;

namespace GUPS.EasyParallelScene
{
    /// <summary>
    /// Use these flags to control the ParallelScene switching.
    /// Auto-Activate rendering and user interaction components.
    /// </summary>
    [Flags]
    public enum ESwitchSceneFlag : byte
    {
        /// <summary>
        /// Just switch to the ParallelScene.
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Switch to the ParallelScene and activate the Main Camera.
        /// </summary>
        ACTIVATE_MAIN_CAMERA = 1,

        /// <summary>
        /// Switch to the ParallelScene and activate the Main AudioListener.
        /// </summary>
        ACTIVATE_MAIN_AUDIOLISTENER = 2,

        /// <summary>
        /// Switch to the ParallelScene and activate the Main EventSystem.
        /// </summary>
        ACTIVATE_MAIN_EVENTSYSTEM = 4,

        /// <summary>
        /// Switch to the ParallelScene and unmute the AudioSources.
        /// </summary>
        ACTIVATE_AUDIOSOURCE = 8,

        /// <summary>
        /// Switch to the ParallelScene and activate the Main Camera, AudioListener, EventSystem and AudioSources.
        /// </summary>
        DEFAULT = ACTIVATE_MAIN_CAMERA | ACTIVATE_MAIN_AUDIOLISTENER | ACTIVATE_MAIN_EVENTSYSTEM | ACTIVATE_AUDIOSOURCE
    }
}