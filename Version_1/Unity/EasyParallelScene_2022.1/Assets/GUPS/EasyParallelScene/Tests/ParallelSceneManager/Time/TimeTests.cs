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

// GUPS
using GUPS.EasyParallelScene.Helper;

namespace GUPS.EasyParallelScene.Tests.ParallelSceneManager
{
    /// <summary>
    /// Tests for the ParallelSceneManager class.
    /// </summary>
    public class TimeTests
    {
        /// <summary>
        /// Start scene path for the tests.
        /// </summary>
        private static String StartScenePath = "Assets/GUPS/EasyParallelScene/Tests/ParallelSceneManager/Time/StartScene.unity";

        /// <summary>
        /// Additive scene path for the tests.
        /// </summary>
        private static String AdditiveScenePath = "Assets/GUPS/EasyParallelScene/Tests/ParallelSceneManager/Time/AdditiveScene.unity";

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
        /// Validate if the ParallelScene got switched too without assigning the main Camera and main AudioListener.
        /// </summary>
        [UnityTest]
        public IEnumerator ValidateTime()
        {
            // Get StartScene.
            Scene var_StartScene = SceneManager.GetSceneByPath(StartScenePath);

            // Check if StartScene is parallel registered.
            Assert.AreEqual(true, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.ContainsParallelScene(StartScenePath));

            // Get StartScene parallel.
            ParallelScene var_StartParallelScene = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.FindParallelScene(StartScenePath);

            // Check if AdditiveScene is parallel registered.
            Assert.AreEqual(true, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.ContainsParallelScene(AdditiveScenePath));

            // Get AdditiveScene parallel.
            ParallelScene var_AdditiveParallelScene = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.FindParallelScene(AdditiveScenePath);

            // Load scene and apply layer.
            AsyncOperation var_LoadingOperation = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.Load(var_AdditiveParallelScene, LoadSceneMode.Additive, ELoadGameObjectFlag.APPLY_LAYER);

            // Wait for loading.
            while (!var_LoadingOperation.isDone)
            {
                yield return null;
            }

            // Validate time scale of the StartScene.
            Assert.AreEqual(1.0f, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.GetTimeScale(var_StartParallelScene));

            // Set time scale of the StartScene.
            GUPS.EasyParallelScene.ParallelSceneManager.Singleton.SetTimeScale(var_StartParallelScene, 0.5f);

            // Validate time scale of the StartScene.
            Assert.AreEqual(0.5f, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.GetTimeScale(var_StartParallelScene));

            // Validate time scale of the AdditiveScene.
            Assert.AreEqual(3.0f, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.GetTimeScale(var_AdditiveParallelScene));

            // Set time scale of the AdditiveScene.
            GUPS.EasyParallelScene.ParallelSceneManager.Singleton.SetTimeScale(var_AdditiveParallelScene, 6.0f);

            // Validate time scale of the AdditiveScene.
            Assert.AreEqual(6.0f, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.GetTimeScale(var_AdditiveParallelScene));

            // Return.
            yield return null;
        }

        /// <summary>
        /// Validate if the ParallelScene got switched too with assigning the main Camera and main AudioListener.
        /// </summary>
        [UnityTest]
        public IEnumerator ValidateIsPaused()
        {
            // Get StartScene.
            Scene var_StartScene = SceneManager.GetSceneByPath(StartScenePath);

            // Check if StartScene is parallel registered.
            Assert.AreEqual(true, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.ContainsParallelScene(StartScenePath));

            // Get StartScene parallel.
            ParallelScene var_StartParallelScene = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.FindParallelScene(StartScenePath);

            // Check if AdditiveScene is parallel registered.
            Assert.AreEqual(true, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.ContainsParallelScene(AdditiveScenePath));

            // Get AdditiveScene parallel.
            ParallelScene var_AdditiveParallelScene = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.FindParallelScene(AdditiveScenePath);

            // Load scene and apply layer.
            AsyncOperation var_LoadingOperation = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.Load(var_AdditiveParallelScene, LoadSceneMode.Additive, ELoadGameObjectFlag.APPLY_LAYER);

            // Wait for loading.
            while (!var_LoadingOperation.isDone)
            {
                yield return null;
            }

            // Validate is paused for StartScene.
            Assert.AreEqual(false, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.GetIsPaused(var_StartParallelScene));

            // Set is paused for StartScene.
            GUPS.EasyParallelScene.ParallelSceneManager.Singleton.SetIsPaused(var_StartParallelScene, true);
            
            // Validate is paused for StartScene.
            Assert.AreEqual(true, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.GetIsPaused(var_StartParallelScene));

            // Validate deltaTime for StartScene.
            Assert.AreEqual(0.0f, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.GetDeltaTime(var_StartParallelScene));

            // Validate is paused for AdditiveScene.
            Assert.AreEqual(false, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.GetIsPaused(var_AdditiveParallelScene));

            // Return.
            yield return null;
        }
    }
}