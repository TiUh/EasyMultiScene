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
    public class SwitchTests
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
        /// Validate if the ParallelScene got switched too without assigning the main Camera and main AudioListener.
        /// </summary>
        [UnityTest]
        public IEnumerator LoadAndSwitchWithoutCameraAndAudio()
        {
            // Get StartScene.
            Scene var_StartScene = SceneManager.GetSceneByPath(StartScenePath);

            // Check if is registered.
            Assert.AreEqual(true, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.ContainsParallelScene(AdditiveScenePath));

            // Get ParallelScene.
            ParallelScene var_ParallelScene = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.FindParallelScene(AdditiveScenePath);

            // Load scene and apply layer.
            AsyncOperation var_LoadingOperation = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.Load(var_ParallelScene, LoadSceneMode.Additive, ELoadGameObjectFlag.APPLY_LAYER);

            // Wait for loading.
            while (!var_LoadingOperation.isDone)
            {
                yield return null;
            }

            // Get the additive Scene.
            Scene var_AdditiveScene = SceneManager.GetSceneByPath(var_ParallelScene.ScenePath);

            // Switch to AdditiveScene.
            GUPS.EasyParallelScene.ParallelSceneManager.Singleton.SwitchTo(var_ParallelScene, ESwitchSceneFlag.NONE, false);

            // Check if active Scene is AdditiveScene.
            Assert.AreEqual(var_AdditiveScene.path, SceneManager.GetActiveScene().path);

            // Check if the Camera is not active.
            Assert.AreEqual(false, CameraHelper.GetMainCamera(var_AdditiveScene).enabled);

            // Check if the Camera is not the main camera.
            Assert.AreNotEqual(Camera.main, CameraHelper.GetMainCamera(var_AdditiveScene));

            // Check if the AudioListener is not active.
            Assert.AreEqual(false, AudioListenerHelper.GetMainAudioListener(var_AdditiveScene).enabled);

            // Check if the AudioListener is not the main AudioListener.
            Assert.AreNotEqual(AudioListenerHelper.GetMainAudioListener(var_StartScene), AudioListenerHelper.GetMainAudioListener(var_AdditiveScene));

            // Return.
            yield return null;
        }

        /// <summary>
        /// Validate if the ParallelScene got switched too with assigning the main Camera and main AudioListener.
        /// </summary>
        [UnityTest]
        public IEnumerator LoadAndSwitchWithCameraAndAudio()
        {
            // Get StartScene.
            Scene var_StartScene = SceneManager.GetSceneByPath(StartScenePath);

            // Check if is registered.
            Assert.AreEqual(true, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.ContainsParallelScene(AdditiveScenePath));

            // Get ParallelScene.
            ParallelScene var_ParallelScene = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.FindParallelScene(AdditiveScenePath);

            // Load scene and apply layer.
            AsyncOperation var_LoadingOperation = GUPS.EasyParallelScene.ParallelSceneManager.Singleton.Load(var_ParallelScene, LoadSceneMode.Additive, ELoadGameObjectFlag.APPLY_LAYER);

            // Wait for loading.
            while (!var_LoadingOperation.isDone)
            {
                yield return null;
            }

            // Get the additive Scene.
            Scene var_AdditiveScene = SceneManager.GetSceneByPath(var_ParallelScene.ScenePath);

            // Switch to AdditiveScene.
            GUPS.EasyParallelScene.ParallelSceneManager.Singleton.SwitchTo(var_ParallelScene, ESwitchSceneFlag.DEFAULT, false);

            // Check if active Scene is AdditiveScene.
            Assert.AreEqual(var_AdditiveScene.path, SceneManager.GetActiveScene().path);

            // Check if the Camera is active.
            Assert.AreEqual(true, CameraHelper.GetMainCamera(var_AdditiveScene).enabled);

            // Check if the Camera is the main camera.
            Assert.AreEqual(Camera.main, CameraHelper.GetMainCamera(var_AdditiveScene));

            // Check if the AudioListener is active.
            Assert.AreEqual(true, AudioListenerHelper.GetMainAudioListener(var_AdditiveScene).enabled);

            // Check if the AudioListener is not the main AudioListener.
            Assert.AreEqual(AudioListenerHelper.GetMainAudioListener(var_AdditiveScene), AudioListenerHelper.GetMainAudioListener(var_AdditiveScene));

            // Return.
            yield return null;
        }
    }
}