using System;
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GUPS.EasyParallelScene
{
    public class ParallelSceneManager : Singleton.PersistentSingleton<ParallelSceneManager>
    {
        /// <summary>
        /// Maps a scene path to a ParallelScene.
        /// </summary>
        private Dictionary<String, ParallelScene> scenePathToParallelScene = new Dictionary<string, ParallelScene>();

        /// <summary>
        /// Maps a scene path to a loaded scene.
        /// </summary>
        private Dictionary<String, Scene> scenePathToScene = new Dictionary<string, Scene>();

        public void Load(ParallelScene _ParallelScene, bool _ApplyLayer)
        {
            // If there is no parallel scene passed, throw an exception.
            if(_ParallelScene == null)
            {
                throw new ArgumentNullException("_ParallelScene");
            }

            // If scene is alread loaded, throw exception. Only single scene is supported!
            if(SceneManager.GetSceneByPath(_ParallelScene.ScenePath) != null || !SceneManager.GetSceneByPath(_ParallelScene.ScenePath).IsValid())
            {
                throw new ArgumentNullException("The scene at path: " + _ParallelScene.ScenePath + " got already loaded!");
            }
            
            // Load the scene. A scene can be loaded more than once!
            SceneManager.LoadScene(_ParallelScene.ScenePath, LoadSceneMode.Additive);

            Scene var_LoadedScene = SceneManager.GetSceneByPath(_ParallelScene.ScenePath);
        }
    }
}