using System;
using System.Collections.Generic;
using System.Linq;

// Unity
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GUPS.EasyParallelScene.Helper
{
    /// <summary>
    /// Helper class for Unity AudioListener.
    /// </summary>
    public static class AudioListenerHelper
    {
        /// <summary>
        /// Returns all AudioListener in all loaded Scenes.
        /// </summary>
        /// <returns>A list of AudioListener in the loaded Scenes.</returns>
        public static List<AudioListener> GetAllAudioListeners()
        {
            List<AudioListener> var_AudioListeners = new List<AudioListener>();

            for(int s = 0; s < SceneManager.sceneCount; s++)
            {
                Scene var_Scene = SceneManager.GetSceneAt(s);
                
                if (var_Scene.isLoaded)
                {
                    var_AudioListeners.AddRange(GetAllAudioListeners(var_Scene));
                }
            }

            return var_AudioListeners;
        }

        /// <summary>
        /// Returns all AudioListeners in the given Scene.
        /// </summary>
        /// <param name="_Scene"></param>
        /// <returns>A list of AudioListeners in the passed _Scene.</returns>
        public static List<AudioListener> GetAllAudioListeners(Scene _Scene)
        {
            return _Scene.GetRootGameObjects().SelectMany((x) => x.GetComponentsInChildren<AudioListener>(true)).ToList();
        }

        /// <summary>
        /// Returns the main AudioListener (Tag equals 'MainCamera') in the given Scene.
        /// </summary>
        /// <param name="_Scene"></param>
        /// <returns>The main AudioListener in the passed _Scene.</returns>
        public static AudioListener GetMainAudioListener(Scene _Scene)
        {
            return GetAllAudioListeners(_Scene).FirstOrDefault((x) => x.CompareTag("MainCamera"));
        }
    }
}
