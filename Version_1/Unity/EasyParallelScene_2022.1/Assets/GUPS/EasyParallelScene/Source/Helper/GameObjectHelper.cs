using System;
using System.Collections.Generic;
using System.Linq;

// Unity
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GUPS.EasyParallelScene.Helper
{
    /// <summary>
    /// Helper class for Unity GameObjects.
    /// </summary>
    public static class GameObjectHelper
    {
        /// <summary>
        /// Returns all active GameObjects inside the ParallelScene _Scene. Returns null if no GameObject was found.
        /// If the passed _Scene is not registered, Unitys default 'Scene.GetRootGameObjects' will be used.
        /// </summary>
        /// <param name="_Scene">The Scene to search GameObjects are in.</param>
        /// <returns></returns>
        public static GameObject[] FindGameObjectsWithLayer(Scene _Scene)
        {
            // Get the ParalleScene.
            ParallelScene var_ParallelScene = ParallelSceneManager.Singleton.FindParallelScene(_Scene.path);

            // If there is no ParalleScene registered. Return the Scene root GameObjects.
            if (var_ParallelScene == null)
            {
                return _Scene.GetRootGameObjects();
            }

            // Get the layer of the _Scene.
            int var_Layer = ParallelSceneManager.Singleton.GetLayer(_Scene);

            // Find the active GameObjects with layer.
            return FindGameObjectsWithLayer(var_Layer);
        }

        /// <summary>
        /// Returns an array of active GameObjects with _Layer.
        /// </summary>
        /// <param name="_Layer">The exact layer to search GameObjects for.</param>
        /// <returns></returns>
        public static GameObject[] FindGameObjectsWithLayer(int _Layer)
        {
            // Result list of active GameObjects in _Layer.
            List<GameObject> var_Result = new List<GameObject>();

            // Find all GameObjects (there is no build in method for all for all of this...)
            UnityEngine.Object[] var_AllObjects = GameObject.FindObjectsOfType(typeof(GameObject));

            // Find all active GameObjects in _Layer.
            for (var i = 0; i < var_AllObjects.Length; i++)
            {
                GameObject var_Current = (GameObject) var_AllObjects[i];

                if (var_Current.activeSelf && var_Current.layer == _Layer)
                {
                    var_Result.Add(var_Current);
                }
            }

            return var_Result.ToArray();
        }

        /// <summary>
        /// Returns an active GameObject tagged _Tag and inside the ParallelScene _Scene. Returns null if no GameObject was found.
        /// If the passed _Scene is not registered, Unitys default 'GameObject.FindGameObjectWithTag' will be used.
        /// </summary>
        /// <param name="_Tag">The name of the tag to search GameObjects for.</param>
        /// <param name="_Scene">The Scene to search GameObjects are in.<</param>
        /// <returns></returns>
        public static GameObject FindGameObjectWithTagAndLayer(String _Tag, Scene _Scene)
        {
            // Get the ParalleScene.
            ParallelScene var_ParallelScene = ParallelSceneManager.Singleton.FindParallelScene(_Scene.path);

            // If there is no ParalleScene registered. Find GameObject without using the layer filter.
            if (var_ParallelScene == null)
            {
                return GameObject.FindGameObjectWithTag(_Tag);
            }

            // Get the layer of the _Scene.
            int var_Layer = ParallelSceneManager.Singleton.GetLayer(_Scene);

            // Find the first GameObject with the given tag and layer.
            return GameObject.FindGameObjectsWithTag(_Tag).FirstOrDefault(_GameObject => _GameObject.layer == var_Layer);
        }

        /// <summary>
        /// Returns an active GameObject tagged _Tag and in ParallelScene with _Layer. Returns empty array if no GameObject was found.
        /// </summary>
        /// <param name="_Tag">The name of the tag to search GameObjects for.</param>
        /// <param name="_Layer">The exact layer to search GameObjects for.</param>
        /// <returns></returns>
        public static GameObject FindGameObjectWithTagAndLayer(String _Tag, int _Layer)
        {
            return GameObject.FindGameObjectsWithTag(_Tag).FirstOrDefault(_GameObject => _GameObject.layer == _Layer);
        }

        /// <summary>
        /// Returns an array of active GameObjects tagged _Tag and in ParallelScene with _Layer. Returns empty array if no GameObject was found.
        /// </summary>
        /// <param name="_Tag">The name of the tag to search GameObjects for.</param>
        /// <param name="_Layer">The exact layer to search GameObjects for.</param>
        /// <returns></returns>
        public static GameObject[] FindGameObjectsWithTagAndLayer(String _Tag, int _Layer)
        {
            return GameObject.FindGameObjectsWithTag(_Tag).Where(_GameObject => _GameObject.layer == _Layer).ToArray();
        }

        /// <summary>
        /// Returns true, if all the passed _GameObjects are in the same Scene.
        /// </summary>
        /// <param name="_GameObjects"></param>
        /// <returns></returns>
        public static bool GameObjectsAreInSameScene(GameObject[] _GameObjects)
        {
            if(_GameObjects.Length == 0)
            {
                return true;
            }

            return _GameObjects.Count(g => g.scene == _GameObjects[0].scene) == _GameObjects.Length;
        }

        /// <summary>
        /// Returns true, if both GameObjects are in the same Scene.
        /// </summary>
        /// <param name="_GameObject1"></param>
        /// <param name="_GameObject2"></param>
        /// <returns></returns>
        public static bool GameObjectsAreInSameScene(GameObject _GameObject1, GameObject _GameObject2)
        {
            return _GameObject1.scene == _GameObject2.scene;
        }
    }
}
