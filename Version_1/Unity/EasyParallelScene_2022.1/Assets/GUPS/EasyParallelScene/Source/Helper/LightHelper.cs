using System;
using System.Collections.Generic;
using System.Linq;

// Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace GUPS.EasyParallelScene.Helper
{
    /// <summary>
    /// Helper class for Unity Lights.
    /// </summary>
    public static class LightHelper
    {
        /// <summary>
        /// Returns all ReflectionProbes in all loaded Scenes.
        /// </summary>
        /// <returns>A list of ReflectionProbes in the loaded Scenes.</returns>
        public static List<ReflectionProbe> GetAllReflectionProbes()
        {
            List<ReflectionProbe> var_ReflectionProbes = new List<ReflectionProbe>();

            for(int s = 0; s < SceneManager.sceneCount; s++)
            {
                Scene var_Scene = SceneManager.GetSceneAt(s);
                
                if (var_Scene.isLoaded)
                {
                    var_ReflectionProbes.AddRange(GetAllReflectionProbes(var_Scene));
                }
            }

            return var_ReflectionProbes;
        }

        /// <summary>
        /// Returns all ReflectionProbes in the given Scene.
        /// </summary>
        /// <param name="_Scene"></param>
        /// <returns>A list of ReflectionProbes in the passed _Scene.</returns>
        public static List<ReflectionProbe> GetAllReflectionProbes(Scene _Scene)
        {
            return _Scene.GetRootGameObjects().SelectMany((x) => x.GetComponentsInChildren<ReflectionProbe>(true)).ToList();
        }

        /// <summary>
        /// Returns all Lights in all loaded Scenes.
        /// </summary>
        /// <returns>A list of Lights in the loaded Scenes.</returns>
        public static List<Light> GetAllLights()
        {
            List<Light> var_Lights = new List<Light>();

            for (int s = 0; s < SceneManager.sceneCount; s++)
            {
                Scene var_Scene = SceneManager.GetSceneAt(s);
                
                if (var_Scene.isLoaded)
                {
                    var_Lights.AddRange(GetAllLights(var_Scene));
                }
            }

            return var_Lights;
        }

        /// <summary>
        /// Returns all Lights in the given Scene.
        /// </summary>
        /// <param name="_Scene"></param>
        /// <returns>A list of Lights in the passed _Scene.</returns>
        public static List<Light> GetAllLights(Scene _Scene)
        {
            return _Scene.GetRootGameObjects().SelectMany((x) => x.GetComponentsInChildren<Light>(true)).ToList();
        }
    }
}
