using System;
using System.Collections.Generic;
using System.Linq;

// Unity - Runtime
using UnityEngine;
using UnityEngine.SceneManagement;

// Unity - Edtior
using UnityEditor;
using UnityEditor.SceneManagement;

namespace GUPS.EasyParallelScene.Editor
{
    /// <summary>
    /// Custom editor for a parallel scene.
    /// </summary>
    [CustomEditor(typeof(ParallelScene))]
    public class ParallelSceneEditor : UnityEditor.Editor
    {
        private SerializedProperty scenePath_Property;

        private SerializedProperty layer_Property;

        private SerializedProperty timeScale_Property;

        private void OnEnable()
        {
            // Link the properties.
            this.scenePath_Property = serializedObject.FindProperty("scenePath");
            this.layer_Property = serializedObject.FindProperty("layer");
            this.timeScale_Property = serializedObject.FindProperty("timeScale");
        }

        public override void OnInspectorGUI()
        {
            // Update to get current data.
            this.serializedObject.Update();

            // Scene - Gui
            SceneAsset var_CurrentScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(this.scenePath_Property.stringValue);

            EditorGUI.BeginChangeCheck();

            SceneAsset var_NewScene = EditorGUILayout.ObjectField(new GUIContent("Scene", "Select a scene you want to make parallelizable."), var_CurrentScene, typeof(SceneAsset), false) as SceneAsset;

            EditorGUILayout.HelpBox("Select a scene you want to make parallelizable.", MessageType.Info);

            if (EditorGUI.EndChangeCheck())
            {
                this.scenePath_Property.stringValue = AssetDatabase.GetAssetPath(var_NewScene);
            }

            // Layer - Gui
            int var_CurrentLayer = this.layer_Property.intValue + 1; // Plus 1 to range from 0 to 32.

            List<String> var_AvailableLayer = new List<String>();

            var_AvailableLayer.Add("-1: None");
            var_AvailableLayer.AddRange(Enumerable.Range(0, 32).Select(index => index + ": " + LayerMask.LayerToName(index)));

            EditorGUI.BeginChangeCheck();

            int var_NewLayer = EditorGUILayout.Popup(new GUIContent("Layer", "Select a unity layer the parallel scene should use. -1 is equals 'null'."), var_CurrentLayer, var_AvailableLayer.ToArray());

            EditorGUILayout.HelpBox("Select a unity layer index the parallel scene should use. -1 is used like a 'null' value. This layer will be used for the camera, collider and gameobjects itself.", MessageType.Info);

            if (EditorGUI.EndChangeCheck())
            {
                this.layer_Property.intValue = var_NewLayer - 1; // Minus 1 to range from -1 to 31.
            }

            // TimeScale - Gui
            float var_CurrentTimeScale = this.timeScale_Property.floatValue;

            EditorGUI.BeginChangeCheck();

            float var_NewTimeScale = EditorGUILayout.FloatField(new GUIContent("TimeScale", "Choose a custom time scale for this parallel scene."), var_CurrentTimeScale);

            EditorGUILayout.HelpBox("Choose a custom time scale for this parallel scene. So you can have multiple scenes running parallel but each can use a custom time scale.", MessageType.Info);

            if (EditorGUI.EndChangeCheck())
            {
                this.layer_Property.floatValue = var_NewTimeScale;
            }

            // Store modified data.
            serializedObject.ApplyModifiedProperties();

            /*if (GUILayout.Button("PressMe"))
            {
                SceneManager.LoadScene(this.scenePath_Property.stringValue);
                //EditorSceneManager.LoadSceneInPlayMode(this.scenePath_Property.stringValue, new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.None));
            }*/
        }
    } 
}