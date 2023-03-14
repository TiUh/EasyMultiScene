using System;
using System.Collections.Generic;
using System.Linq;

// Unity
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GUPS.EasyParallelScene.Helper
{
    /// <summary>
    /// Helper class for Unity Cameras.
    /// </summary>
    public static class CameraHelper
    {
        /// <summary>
        /// Returns all Cameras in all loaded Scenes.
        /// </summary>
        /// <returns>A list of Cameras in the loaded Scenes.</returns>
        public static List<Camera> GetAllCameras()
        {
            List<Camera> var_Cameras = new List<Camera>();

            for(int s = 0; s < SceneManager.sceneCount; s++)
            {
                Scene var_Scene = SceneManager.GetSceneAt(s);
                
                if (var_Scene.isLoaded)
                {
                    var_Cameras.AddRange(GetAllCameras(var_Scene));
                }
            }

            return var_Cameras;
        }

        /// <summary>
        /// Returns all Cameras in the given Scene.
        /// </summary>
        /// <param name="_Scene"></param>
        /// <returns>A list of Cameras in the passed _Scene.</returns>
        public static List<Camera> GetAllCameras(Scene _Scene)
        {
            return _Scene.GetRootGameObjects().SelectMany((x) => x.GetComponentsInChildren<Camera>(true)).ToList();
        }

        /// <summary>
        /// Returns the main Camera (Tag equals 'MainCamera') in the given Scene.
        /// </summary>
        /// <param name="_Scene"></param>
        /// <returns>The main Camera in the passed _Scene.</returns>
        public static Camera GetMainCamera(Scene _Scene)
        {
            return GetAllCameras(_Scene).FirstOrDefault((x) => x.CompareTag("MainCamera"));
        }
    }
}
