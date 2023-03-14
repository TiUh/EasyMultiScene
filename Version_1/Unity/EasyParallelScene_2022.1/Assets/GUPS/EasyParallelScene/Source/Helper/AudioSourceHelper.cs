using System;
using System.Collections.Generic;
using System.Linq;

// Unity
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GUPS.EasyParallelScene.Helper
{
    /// <summary>
    /// Helper class for Unity EventSystems.
    /// </summary>
    public static class AudioSourceHelper
    {
        /// <summary>
        /// Returns all AudioSources in all loaded Scenes.
        /// </summary>
        /// <returns>A list of AudioSources in the loaded Scenes.</returns>
        public static List<AudioSource> GetAllAudioSources()
        {
            List<AudioSource> var_AudioSources = new List<AudioSource>();

            for(int s = 0; s < SceneManager.sceneCount; s++)
            {
                Scene var_Scene = SceneManager.GetSceneAt(s);
                
                if (var_Scene.isLoaded)
                {
                    var_AudioSources.AddRange(GetAllAudioSources(var_Scene));
                }
            }

            return var_AudioSources;
        }

        /// <summary>
        /// Returns all AudioSources in the given Scene.
        /// </summary>
        /// <param name="_Scene"></param>
        /// <returns>A list of AudioSources in the passed _Scene.</returns>
        public static List<AudioSource> GetAllAudioSources(Scene _Scene)
        {
            return _Scene.GetRootGameObjects().SelectMany((x) => x.GetComponentsInChildren<AudioSource>(true)).ToList();
        }
    }
}
