using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace ModularBuildingsFramework.Roofs
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(StandardFlatBoxRoof))]
    public class StandardFlatBoxRoofEditor : Editor
	{
        /// ======================================================================

        private SerializedProperty length;
        private SerializedProperty depth;
        private SerializedProperty mode;
        private SerializedProperty prefab;

        /// ======================================================================

        private void OnEnable()
        {
            var parameters = serializedObject.FindProperty("_parameters");
            length = parameters.FindPropertyRelative("_length");
            depth = parameters.FindPropertyRelative("_depth");
            mode = parameters.FindPropertyRelative("_mode");
            prefab = serializedObject.FindProperty("_prefab");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(length, new GUIContent("Length:"));
            EditorGUILayout.PropertyField(depth, new GUIContent("Depth:"));
            EditorGUILayout.PropertyField(mode, new GUIContent("Mode:"));
            EditorGUILayout.PropertyField(prefab, new GUIContent("Prefab:"));

            serializedObject.ApplyModifiedProperties();
        }

        /// ======================================================================
    }
}