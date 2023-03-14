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
    public class CreateTests
    {
        /// <summary>
        /// Start scene path for the tests.
        /// </summary>
        private static String StartScenePath = "Assets/GUPS/EasyParallelScene/Tests/ParallelSceneManager/Create/StartScene.unity";

        /// <summary>
        /// The ParallelSceneManager used for the tests.
        /// </summary>
        private GUPS.EasyParallelScene.ParallelSceneManager manager;

        /// <summary>
        /// Setup before each test.
        /// </summary>
        [SetUp]
        public void Init()
        {
            // Go to the start test scene.
            EditorSceneManager.LoadSceneInPlayMode(StartScenePath, new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.None));

            // Create a new ParallelSceneManager.
            manager = GUPS.EasyParallelScene.ParallelSceneManager.Singleton;
        }

        /// <summary>
        /// Clean up after each test.
        /// </summary>
        [TearDown]
        public void CleanUp()
        {
            // Delete the ParallelSceneManager.
            GameObject.DestroyImmediate(this.manager.gameObject);
        }

        /// <summary>
        /// Validate if the ParallelSceneManager got created.
        /// </summary>
        [Test]
        public void ValidateCreation()
        {
            Assert.AreEqual(true, GUPS.EasyParallelScene.ParallelSceneManager.Exists);
        }
    }
}