using System;
using System.Collections;
using System.Linq;

// Test
using NUnit.Framework;

// Unity - Runtime
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

// Unity - Editor
using UnityEditor;
using UnityEditor.SceneManagement;

namespace GUPS.EasyParallelScene.Tests.ParallelSceneManager
{
    /// <summary>
    /// Tests for the ParallelSceneManager class.
    /// </summary>
    public class LoadTests
    {
        /// <summary>
        /// Start scene path for the tests.
        /// </summary>
        private static String StartScenePath = "Assets/GUPS/EasyParallelScene/Tests/ParallelSceneManager/Load/StartScene.unity";

        /// <summary>
        /// Additive scene path for the tests.
        /// </summary>
        private static String AdditiveScenePath = "Assets/GUPS/EasyParallelScene/Tests/ParallelSceneManager/Load/AdditiveScene.unity";

        /// <summary>
        /// Setup before each test.
        /// </summary>
        [SetUp]
        public void Init()
        {
            // Go to the start test scene.
            EditorSceneManager.LoadSceneInPlayMode(StartScenePath, new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.None));
        }

        /// <summary>
        /// Clean up after each test.
        /// </summary>
        [TearDown]
        public void CleanUp()
        {
            // Delete the ParallelSceneManager.
            GameObject.DestroyImmediate(GUPS.EasyParallelScene.ParallelSceneManager.Singleton);
        }

        /// <summary>
        /// Validate if the ParallelScene got loaded without applying the layer.
        /// </summary>
        [UnityTest]
        public IEnumerator LoadSingleAndNotApplyLayer()
        {
            // Check if is registered.
            Assert.AreEqual(true, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.ContainsParallelScene(AdditiveScenePath));

            // Get ParallelScene.
            ParallelScene var_ParallelScene = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.FindParallelScene(AdditiveScenePath);

            // Load Scene and do not apply layer.
            AsyncOperation var_LoadingOperation = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.Load(var_ParallelScene, LoadSceneMode.Single, ELoadGameObjectFlag.NONE);

            // Wait for loading.
            while (!var_LoadingOperation.isDone)
            {
                yield return null;
            }

            // Get the single Scene.
            Scene var_SingleScene = SceneManager.GetSceneByPath(var_ParallelScene.ScenePath);

            Assert.AreEqual(1, SceneManager.sceneCount);

            GameObject[] var_RootGameObjects = var_SingleScene.GetRootGameObjects();

            Assert.AreEqual(0, var_RootGameObjects.Count(g => g.layer == var_ParallelScene.Layer));

            // Return.
            yield return null;
        }

        /// <summary>
        /// Validate if the ParallelScene got loaded with applying the layer.
        /// </summary>
        [UnityTest]
        public IEnumerator LoadSingleAndApplyLayer()
        {
            // Check if is registered.
            Assert.AreEqual(true, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.ContainsParallelScene(AdditiveScenePath));

            // Get ParallelScene.
            ParallelScene var_ParallelScene = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.FindParallelScene(AdditiveScenePath);

            // Load scene and do not apply layer.
            AsyncOperation var_LoadingOperation = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.Load(var_ParallelScene, LoadSceneMode.Single, ELoadGameObjectFlag.APPLY_LAYER);

            // Wait for loading.
            while (!var_LoadingOperation.isDone)
            {
                yield return null;
            }
            
            // Get the single Scene.
            Scene var_SingleScene = SceneManager.GetSceneByPath(var_ParallelScene.ScenePath);
            
            Assert.AreEqual(1, SceneManager.sceneCount);

            GameObject[] var_RootGameObjects = var_SingleScene.GetRootGameObjects();

            Assert.AreEqual(var_RootGameObjects.Length, var_RootGameObjects.Count(g => g.layer == var_ParallelScene.Layer));

            // Return.
            yield return null;
        }

        /// <summary>
        /// Validate if the ParallelScene got loaded without applying the layer.
        /// </summary>
        [UnityTest]
        public IEnumerator LoadAndNotApplyLayer()
        {
            // Check if is registered.
            Assert.AreEqual(true, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.ContainsParallelScene(AdditiveScenePath));

            // Get ParallelScene.
            ParallelScene var_ParallelScene = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.FindParallelScene(AdditiveScenePath);

            // Load scene and do not apply layer.
            AsyncOperation var_LoadingOperation = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.Load(var_ParallelScene, LoadSceneMode.Additive, ELoadGameObjectFlag.NONE);

            // Wait for AdditiveScene.
            while (!var_LoadingOperation.isDone)
            {
                yield return null;
            }
            
            // Get the additive Scene.
            Scene var_AdditiveScene = SceneManager.GetSceneByPath(var_ParallelScene.ScenePath);

            GameObject[] var_RootGameObjects = var_AdditiveScene.GetRootGameObjects();

            Assert.AreEqual(0, var_RootGameObjects.Count(g => g.layer == var_ParallelScene.Layer));

            // Return.
            yield return null;
        }

        /// <summary>
        /// Validate if the ParallelScene got loaded with applying the layer.
        /// </summary>
        [UnityTest]
        public IEnumerator LoadAndApplyLayer()
        {
            // Check if is registered.
            Assert.AreEqual(true, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.ContainsParallelScene(AdditiveScenePath));

            // Get ParallelScene.
            ParallelScene var_ParallelScene = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.FindParallelScene(AdditiveScenePath);

            // Load scene and do not apply layer.
            AsyncOperation var_LoadingOperation = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.Load(var_ParallelScene, LoadSceneMode.Additive, ELoadGameObjectFlag.APPLY_LAYER);

            // Wait for AdditiveScene.
            while (!var_LoadingOperation.isDone)
            {
                yield return null;
            }

            // Get the additive Scene.
            Scene var_AdditiveScene = SceneManager.GetSceneByPath(var_ParallelScene.ScenePath);

            GameObject[] var_RootGameObjects = var_AdditiveScene.GetRootGameObjects();

            Assert.AreEqual(var_RootGameObjects.Length, var_RootGameObjects.Count(g => g.layer == var_ParallelScene.Layer));

            // Return.
            yield return null;
        }

        /// <summary>
        /// Validate if the ParallelScene got loaded and merged without applying the layer.
        /// </summary>
        [UnityTest]
        public IEnumerator LoadAndMerge()
        {
            // Check if is registered.
            Assert.AreEqual(true, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.ContainsParallelScene(AdditiveScenePath));

            // Get ParallelScene.
            ParallelScene var_ParallelScene = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.FindParallelScene(AdditiveScenePath);

            // Load scene and do not apply layer.
            AsyncOperation var_LoadingOperation = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.Load(var_ParallelScene, LoadSceneMode.Additive, ELoadGameObjectFlag.MERGE);

            // Wait for loading.
            while (!var_LoadingOperation.isDone)
            {
                yield return null;
            }

            // Get the additive Scene.
            Scene var_AdditiveScene = SceneManager.GetSceneByPath(var_ParallelScene.ScenePath);

            while (!var_AdditiveScene.isSubScene)
            {
                yield return null;
            }

            // Check if all GameObjects have a different layer.
            Scene var_StartScene = SceneManager.GetSceneByPath(StartScenePath);

            GameObject[] var_RootGameObjects = var_StartScene.GetRootGameObjects();

            Assert.AreEqual(0, var_RootGameObjects.Count(g => g.layer == var_ParallelScene.Layer));

            // Check if there is only one scene loaded.
            Assert.AreEqual(1, SceneManager.sceneCount);

            // Return.
            yield return null;
        }


        /// <summary>
        /// Validate if the ParallelScene got loaded with applying the layer.
        /// </summary>
        [UnityTest]
        public IEnumerator LoadAndMergeAndApplyLayer()
        {
            // Check if is registered.
            Assert.AreEqual(true, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.ContainsParallelScene(AdditiveScenePath));

            // Get ParallelScene.
            ParallelScene var_ParallelScene = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.FindParallelScene(AdditiveScenePath);

            // Load scene and do not apply layer.
            AsyncOperation var_LoadingOperation = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.Load(var_ParallelScene, LoadSceneMode.Additive, ELoadGameObjectFlag.APPLY_LAYER | ELoadGameObjectFlag.MERGE);
            
            // Wait for loading.
            while (!var_LoadingOperation.isDone)
            {
                yield return null;
            }

            // Get the additive Scene.
            Scene var_AdditiveScene = SceneManager.GetSceneByPath(var_ParallelScene.ScenePath);

            while (!var_AdditiveScene.isSubScene)
            {
                yield return null;
            }

            // Get Start Scene.
            Scene var_StartScene = SceneManager.GetSceneByPath(StartScenePath);

            // Get Start ParallelScene.
            ParallelScene var_ParallelStartScene = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.FindParallelScene(StartScenePath);

            // Check if all GameObjects have a different layer.
            GameObject[] var_RootGameObjects = var_StartScene.GetRootGameObjects();

            Assert.AreEqual(var_RootGameObjects.Length, var_RootGameObjects.Count(g => g.layer == var_ParallelStartScene.Layer));

            // Check if there is only one scene loaded.
            Assert.AreEqual(1, SceneManager.sceneCount);

            // Return.
            yield return null;
        }
    }
}