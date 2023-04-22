using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace ModularBuildingsFramework.Columns
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(StandardColumn))]
    public class StandardColumnEditor : Editor
    {
        /// ======================================================================

        private ReorderableList bottomItems;
        private ReorderableList middleItems;
        private ReorderableList topItems;

        private const string bottomCollectionField = "_bottom";
        private const string middleCollectionField = "_middle";
        private const string topCollectionField = "_top";
        private const string itemsField = "_items";

        private const string builderField = "_builder";
        private const string isRequiredField = "_isRequired";

        private static readonly GUIContent bottomHeaderContent = new GUIContent("Bottom Elements");
        private static readonly GUIContent middleHeaderContent = new GUIContent("Middle Elements");
        private static readonly GUIContent topHeaderContent = new GUIContent("Top Elements");

        private static readonly GUIContent builderContent = GUIContent.none;
        private static readonly GUIContent isRequiredContent = new GUIContent("Is Required");

        private const float builderOffset = 0f;
        private const float builderScale = 0.5f;
        private const float isRequiredOffset = 0.53f;
        private const float isRequiredScale = 0.45f;

        private const int elementTopPadding = 2;
        private static readonly Type builerType = typeof(IElementBuilder);

        /// ======================================================================

        private void OnEnable()
        {
            var collection = serializedObject.FindProperty(bottomCollectionField);
            bottomItems = new ReorderableList(
                serializedObject,
                collection.FindPropertyRelative(itemsField),
                true,
                true,
                true,
                true);

            collection = serializedObject.FindProperty(middleCollectionField);
            middleItems = new ReorderableList(
                 serializedObject,
                 collection.FindPropertyRelative(itemsField),
                 true,
                 true,
                 true,
                 true);

            collection = serializedObject.FindProperty(topCollectionField);
            topItems = new ReorderableList(
                serializedObject,
                collection.FindPropertyRelative(itemsField),
                true,
                true,
                true,
                true);

            bottomItems.drawHeaderCallback += OnBottomDrawHeader;
            bottomItems.drawElementCallback += OnBottomDrawElement;
            bottomItems.onAddCallback += OnBottomElementAdd;

            middleItems.drawHeaderCallback += OnMiddleDrawHeader;
            middleItems.drawElementCallback += OnMiddleDrawElement;
            middleItems.onAddCallback += OnMiddleElementAdd;

            topItems.drawHeaderCallback += OnTopDrawHeader;
            topItems.drawElementCallback += OnTopDrawElement;
            topItems.onAddCallback += OnTopElementAdd;
        }
        private void OnDisable()
        {
            bottomItems.drawHeaderCallback -= OnBottomDrawHeader;
            bottomItems.drawElementCallback -= OnBottomDrawElement;
            bottomItems.onAddCallback -= OnBottomElementAdd;

            middleItems.drawHeaderCallback -= OnMiddleDrawHeader;
            middleItems.drawElementCallback -= OnMiddleDrawElement;
            middleItems.onAddCallback -= OnMiddleElementAdd;

            topItems.drawHeaderCallback -= OnTopDrawHeader;
            topItems.drawElementCallback -= OnTopDrawElement;
            topItems.onAddCallback -= OnTopElementAdd;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            bottomItems.DoLayoutList();

            EditorGUILayout.Space();
            middleItems.DoLayoutList();

            EditorGUILayout.Space();
            topItems.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        /// ======================================================================

        private void OnBottomDrawHeader(Rect rect)
        {
            DrawHeader(rect, bottomHeaderContent);
        }
        private void OnMiddleDrawHeader(Rect rect)
        {
            DrawHeader(rect, middleHeaderContent);
        }
        private void OnTopDrawHeader(Rect rect)
        {
            DrawHeader(rect, topHeaderContent);
        }

        private void OnBottomDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            DrawElement(bottomItems, rect, index, isActive, isFocused, true);
        }
        private void OnMiddleDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            DrawElement(middleItems, rect, index, isActive, isFocused, false);
        }
        private void OnTopDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            DrawElement(topItems, rect, index, isActive, isFocused, true);
        }

        private void OnBottomElementAdd(ReorderableList list)
        {
            AddElement(list);
            SetRequired(list, true);
        }
        private void OnMiddleElementAdd(ReorderableList list)
        {
            AddElement(list);
            SetRequired(list, false);
        }
        private void OnTopElementAdd(ReorderableList list)
        {
            AddElement(list);
            SetRequired(list, true);
        }

        /// ======================================================================

        private static void DrawHeader(Rect rect, GUIContent content)
        {
            EditorGUI.LabelField(rect, content);
        }
        private static void DrawElement(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused, bool isDisableRequiredChange)
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);

            var area = new Rect(
                rect.x + rect.width * builderOffset,
                rect.y + elementTopPadding,
                rect.width * builderScale,
                EditorGUIUtility.singleLineHeight);

            var prop = element.FindPropertyRelative(builderField);
            prop.objectReferenceValue = EditorGUI.ObjectField(
                area, 
                builderContent, 
                prop.objectReferenceValue,
                builerType, 
                false);

            if (isDisableRequiredChange)
                GUI.enabled = false;

            area = new Rect(
                rect.x + rect.width * isRequiredOffset,
                rect.y + elementTopPadding,
                rect.width * isRequiredScale,
                EditorGUIUtility.singleLineHeight);

            prop = element.FindPropertyRelative(isRequiredField);
            prop.boolValue = EditorGUI.ToggleLeft(area, isRequiredContent, prop.boolValue);

            if (isDisableRequiredChange)
                GUI.enabled = true;
        }

        private static void AddElement(ReorderableList list)
        {
            list.serializedProperty.arraySize++;
        }
        private static void SetRequired(ReorderableList list, bool value)
        {
            var index = list.serializedProperty.arraySize - 1;
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative(isRequiredField).boolValue = value;
        }

        /// ======================================================================
    }
}