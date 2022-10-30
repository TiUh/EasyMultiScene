using System;

// Test
using NUnit.Framework;

// Unity - Runtime
using UnityEngine;
using UnityEngine.TestTools;

namespace GUPS.EasyParallelScene.Tests
{
    /// <summary>
    /// Tests the ParallelSceneManager class.
    /// </summary>
    public class ParallelSceneManagerTests
    {
        /// <summary>
        /// The ParallelSceneManager used for the tests.
        /// </summary>
        private ParallelSceneManager manager;

        /// <summary>
        /// Setup before each test.
        /// </summary>
        [SetUp]
        public void Init()
        {
            // Create a new ParallelSceneManager.
            manager = ParallelSceneManager.Singleton;
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

        [Test]
        public void Load()
        {
        }
    }
}