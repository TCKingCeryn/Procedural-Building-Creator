using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace ModularBuildingsFramework.Elements.Standard
{
    // DONE
    [CanEditMultipleObjects]
    [CustomEditor(typeof(StandardElement))]
    public class StandardElementEditor : Editor
    {
        /// ======================================================================

        private SerializedProperty length;
        private SerializedProperty height;
        private SerializedProperty depth;
        private SerializedProperty isHorizontalMrror;
        private SerializedProperty isVerticalMirror;
        private ReorderableList items;

        private const string parametersField = "_parameters";
        private const string lengthField = "_length";
        private const string heightField = "_height";
        private const string depthField = "_depth";
        private const string isHorizontalMrrorField = "_isHorizontalMrror";
        private const string isVerticalMirrorField = "_isVerticalMirror";

        private const string collectionField = "_items";
        private const string itemsField = "_items";
        private const string prefabField = "_prefab";
        private const string weightField = "_weight";
        
        private readonly GUIContent lengthContent = new GUIContent("Length:");
        private readonly GUIContent heightContent = new GUIContent("Height:");
        private readonly GUIContent depthContent = new GUIContent("Depth:");
        private readonly GUIContent isHorizontalMrrorContent = new GUIContent("Is Horizontal Mrror:");
        private readonly GUIContent isVerticalMirrorContent = new GUIContent("Is Vertical Mirror:");

        private readonly GUIContent itemsHeaderContent = new GUIContent("Prefabs");
        private readonly GUIContent prefabContent = GUIContent.none;
        private readonly GUIContent weightContent = GUIContent.none;

        private const float prefabPadding = 0.0f;
        private const float prefabScale = 0.45f;
        private const float weightPadding = 0.5f;
        private const float weightScale = 0.5f;

        private const int elementTopPadding = 2;

        /// ======================================================================

        private void OnEnable()
        {
            var parameters = serializedObject.FindProperty(parametersField);
            length = parameters.FindPropertyRelative(lengthField);
            height = parameters.FindPropertyRelative(heightField);
            depth = parameters.FindPropertyRelative(depthField);
            isHorizontalMrror = parameters.FindPropertyRelative(isHorizontalMrrorField);
            isVerticalMirror = parameters.FindPropertyRelative(isVerticalMirrorField);

            var collection = serializedObject.FindProperty(collectionField);
            items = new ReorderableList(
                serializedObject,
                collection.FindPropertyRelative(itemsField),
                true,
                true,
                true,
                true);

            items.drawHeaderCallback += OnItemsDrawHeader;
            items.drawElementCallback += OnItemsDrawElement;
            items.onAddCallback += OnItemsElementAdd;
        }
        private void OnDisable()
        {
            items.drawHeaderCallback -= OnItemsDrawHeader;
            items.drawElementCallback -= OnItemsDrawElement;
            items.onAddCallback -= OnItemsElementAdd;
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(length, lengthContent);
            EditorGUILayout.PropertyField(height, heightContent);
            EditorGUILayout.PropertyField(depth, depthContent);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(isHorizontalMrror, isHorizontalMrrorContent);
            EditorGUILayout.PropertyField(isVerticalMirror, isVerticalMirrorContent);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            items.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        /// ======================================================================

        private void OnItemsDrawHeader(Rect rect)
        {
            DrawItemsHeader(rect);
        }
        private void OnItemsDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            DrawItemsElement(items, rect, index, isActive, isFocused);
        }
        private void OnItemsElementAdd(ReorderableList list)
        {
            AddElement(list);
            SetElementWeight(list, 100);
        }

        private void DrawItemsHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, itemsHeaderContent);
        }
        private void DrawItemsElement(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);

            var area = new Rect(
                rect.x + rect.width * prefabPadding,
                rect.y + elementTopPadding,
                rect.width * prefabScale,
                EditorGUIUtility.singleLineHeight);

            var prop = element.FindPropertyRelative(prefabField);
            EditorGUI.PropertyField(area, prop, prefabContent);

            area = new Rect(
                rect.x + rect.width * weightPadding,
                rect.y + elementTopPadding,
                rect.width * weightScale,
                EditorGUIUtility.singleLineHeight);

            prop = element.FindPropertyRelative(weightField);
            EditorGUI.IntSlider(area, prop, 0, 100, weightContent);
        }

        private void AddElement(ReorderableList list)
        {
            list.serializedProperty.arraySize++;
        }
        private void SetElementWeight(ReorderableList list, int weight)
        {
            var index = list.serializedProperty.arraySize - 1;
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative(weightField).intValue = weight;
        }

        /// ======================================================================
    }
}