using System;

// Test
using NUnit.Framework;

// Unity - Runtime
using UnityEngine;
using UnityEngine.TestTools;

// Unity - Editor
using UnityEditor;

namespace GUPS.EasyParallelScene.Editor.Tests.ParallelScene
{
    /// <summary>
    /// Tests the ParallelScene class.
    /// </summary>
    public class ParallelSceneTests
    {
        /// <summary>
        /// This test creates and then deletes a ParallelScene object.
        /// </summary>
        [Test]
        public void CreateAndDeleteParallelScene()
        {
            // Create
            GUPS.EasyParallelScene.ParallelScene var_ParallelScene = ScriptableObject.CreateInstance<GUPS.EasyParallelScene.ParallelScene>();

            String var_Path = AssetDatabase.GenerateUniqueAssetPath("Assets/GUPS/EasyParallelScene/Editor/Tests/ParallelScene/ParallelScene.asset");
            AssetDatabase.CreateAsset(var_ParallelScene, var_Path);

            AssetDatabase.SaveAssets();

            // Delete
            AssetDatabase.DeleteAsset(var_Path);

            AssetDatabase.SaveAssets();
        }
    }
}