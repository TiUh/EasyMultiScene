using System;
using System.Collections.Generic;
using System.Linq;

// Unity - Runtime
using UnityEngine;

// Unity - Editor
using UnityEditor;

namespace GUPS.EasyParallelScene.Editor
{
    /// <summary>
    /// Custom editor for a ParallelScene.
    /// </summary>
    [CustomEditor(typeof(ParallelScene))]
    public class ParallelSceneEditor : UnityEditor.Editor
    {
        private SerializedProperty scenePath_Property;

        private SerializedProperty layer_Property;

        private SerializedProperty useCustomTimeScale_Property;
        
        private SerializedProperty timeScale_Property;

        private void OnEnable()
        {
            // Link the properties.
            this.scenePath_Property = serializedObject.FindProperty("scenePath");
            this.layer_Property = serializedObject.FindProperty("layer");
            this.useCustomTimeScale_Property = serializedObject.FindProperty("useCustomTimeScale");
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
            int var_CurrentLayer = this.layer_Property.intValue;

            EditorGUI.BeginChangeCheck();

            int var_NewLayer = EditorGUILayout.LayerField(new GUIContent("Layer", "Select a unity layer the parallel scene should use."), var_CurrentLayer);

            EditorGUILayout.HelpBox("Select a unity layer index the parallel scene should use. This layer will be used for the camera, collider and gameobjects itself.", MessageType.Info);

            if (EditorGUI.EndChangeCheck())
            {
                this.layer_Property.intValue = var_NewLayer;
            }

            // UseCustomTimeScale - Gui
            bool var_CurrentUseCustomTimeScale = this.useCustomTimeScale_Property.boolValue;

            EditorGUI.BeginChangeCheck();
            
            bool var_NewUseCustomTimeScale = EditorGUILayout.Toggle(new GUIContent("CustomTimeScale", "If true, the parallel scene may use a custom time scale beside the Unity Time.TimeScale. However, this also has some disadvantages. You can only effect the delta time you can access in code. Animations / Navigation / Sounds / ... are not effected!"), var_CurrentUseCustomTimeScale);

            EditorGUILayout.HelpBox("If true, the parallel scene may use a custom time scale beside the Unity Time.TimeScale. However, this also has some disadvantages. You can only effect the delta time you can access in code. Animations / Navigation / Sounds / ... are not effected!", MessageType.Info);

            if (EditorGUI.EndChangeCheck())
            {
                this.useCustomTimeScale_Property.boolValue = var_NewUseCustomTimeScale;
            }

            // TimeScale - Gui
            if (var_NewUseCustomTimeScale)
            {
                float var_CurrentTimeScale = this.timeScale_Property.floatValue;

                EditorGUI.BeginChangeCheck();

                float var_NewTimeScale = EditorGUILayout.FloatField(new GUIContent("TimeScale", "Choose a custom time scale for this parallel scene."), var_CurrentTimeScale);

                EditorGUILayout.HelpBox("Choose a custom time scale for this parallel scene. So you can have multiple scenes running parallel but each can use a custom time scale.", MessageType.Info);

                if (EditorGUI.EndChangeCheck())
                {
                    this.timeScale_Property.floatValue = var_NewTimeScale;
                }
            }

            // Store modified data.
            serializedObject.ApplyModifiedProperties();
        }
    } 
}