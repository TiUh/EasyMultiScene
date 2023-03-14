using System;

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
    public class RegisterTests
    {
        /// <summary>
        /// Scene path for the test.
        /// </summary>
        private static String StartScenePath = "Assets/GUPS/EasyParallelScene/Tests/ParallelSceneManager/Register/StartScene.unity";

        /// <summary>
        /// ParallelScene asset path.
        /// </summary>
        private static String AssetPath = "Assets/GUPS/EasyParallelScene/Tests/ParallelSceneManager/Register/ParallelStartScene.asset";

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
        /// Validate if the ParallelScene was successfully registered.
        /// </summary>
        [Test]
        public void RegisterAndContains()
        {
            // Is not registered yet.
            Assert.AreEqual(false, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.ContainsParallelScene(StartScenePath));

            // Find the ParallelScene assets.
            ParallelScene var_ParallelScene = AssetDatabase.LoadAssetAtPath(AssetPath, typeof(ParallelScene)) as ParallelScene;

            // Check if asset exists.
            Assert.AreNotEqual(null, var_ParallelScene);

            // Register.
            GUPS.EasyParallelScene.ParallelSceneManager.Singleton.RegisterParallelScene(var_ParallelScene);

            // Is now registered.
            Assert.AreEqual(true, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.ContainsParallelScene(StartScenePath));
            Assert.AreNotEqual(null, GUPS.EasyParallelScene.ParallelSceneManager.Singleton.FindParallelScene(StartScenePath));
        }
    }
}