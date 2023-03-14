// Unity - Runtime
using UnityEngine;

// Unity - Editor
using UnityEditor;
using UnityEditorInternal;

namespace GUPS.EasyParallelScene.Editor
{
    /// <summary>
    /// Custom editor for a ParallelSceneManager.
    /// </summary>
    [CustomEditor(typeof(ParallelSceneManager))]
    public class ParallelSceneManagerEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Stores the parallelScenes list as SerializedProperty.
        /// </summary>
        private SerializedProperty parallelScenes_Property;

        /// <summary>
        /// UI-Element for a editor list.
        /// </summary>
        private ReorderableList parallelScene_ReorderableList;

        /// <summary>
        /// Init all SerializedPropertys and UI-Elements.
        /// </summary>
        private void OnEnable()
        {
            // Link the properties.
            this.parallelScenes_Property = serializedObject.FindProperty("parallelScenes");

            // Init the editor list.
            this.parallelScene_ReorderableList = new ReorderableList(this.serializedObject, this.parallelScenes_Property, true, true, true, true);

            // Init the editor list rendering.
            this.parallelScene_ReorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, new GUIContent("ParallelScenes", "Assign here all ParallelScenes you want to make available while runtime. If a ParallelScene is not registered here, it cannot be used."));
            this.parallelScene_ReorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.y += 2f;
                rect.height = EditorGUIUtility.singleLineHeight;

                GUIContent objectLabel = new GUIContent($"ParallelScene {index}");

                EditorGUI.PropertyField(rect, this.parallelScene_ReorderableList.serializedProperty.GetArrayElementAtIndex(index), objectLabel);
            };
        }

        /// <summary>
        /// Render the UI-Elements.
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Update to get current data.
            this.serializedObject.Update();

            // Render.
            this.parallelScene_ReorderableList.DoLayoutList();

            // Store modified data.
            serializedObject.ApplyModifiedProperties();
        }
    } 
}