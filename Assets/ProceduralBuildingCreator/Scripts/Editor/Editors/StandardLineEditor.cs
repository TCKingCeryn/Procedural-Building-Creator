using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace ModularBuildingsFramework.Lines
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(StandardLine))]
    public class StandardLineEditor : Editor
	{
        /// ======================================================================

        private ReorderableList startProp;
        private ReorderableList middleProp;
        private ReorderableList finishProp;

        private const string startField = "_start";
        private const string middleField = "_middle";
        private const string finishField = "_finish";
        private const string itemsField = "_items";

        private const string builderField = "_builder";
        private const string isRequiredField = "_isRequired";
        private const string isHorizontalMirrorField = "_isHorizontalMirror";

        private static readonly GUIContent startHeaderContent = new GUIContent("Start Columns");
        private static readonly GUIContent middleHeaderContent = new GUIContent("Middle Columns");
        private static readonly GUIContent finishHeaderContent = new GUIContent("Finish Columns");

        private static readonly GUIContent builderContent = GUIContent.none;
        private static readonly GUIContent isRequiredContent = new GUIContent("Is Required");
        private static readonly GUIContent isHorizontalMirrorContent = new GUIContent("Is Mirror");

        private const float bulderOffset = 0f;
        private const float builderScale = 0.5f;

        private const float mirrorOffset = 0.53f;
        private const float mirrorScale = 0.2f;

        private const float requiredOffset = 0.75f;
        private const float requiredScale = 0.25f;

        private const int elementTopPadding = 2;
        private static readonly Type builerType = typeof(IColumnBuilder);

        /// ======================================================================

        private void OnEnable()
        {
            var collection = serializedObject.FindProperty(startField);
            startProp = new ReorderableList(
                serializedObject,
                collection.FindPropertyRelative(itemsField),
                true,
                true,
                true,
                true);

            collection = serializedObject.FindProperty(middleField);
            middleProp = new ReorderableList(
                 serializedObject,
                 collection.FindPropertyRelative(itemsField),
                 true,
                 true,
                 true,
                 true);

            collection = serializedObject.FindProperty(finishField);
            finishProp = new ReorderableList(
                serializedObject,
                collection.FindPropertyRelative(itemsField),
                true,
                true,
                true,
                true);

            startProp.drawHeaderCallback += OnStartDrawHeader;
            startProp.drawElementCallback += OnStartDrawElement;
            startProp.onAddCallback += OnStartElementAdd;

            middleProp.drawHeaderCallback += OnMiddleDrawHeader;
            middleProp.drawElementCallback += OnMiddleDrawElement;
            middleProp.onAddCallback += OnMiddleElementAdd;

            finishProp.drawHeaderCallback += OnFinishDrawHeader;
            finishProp.drawElementCallback += OnFinishDrawElement;
            finishProp.onAddCallback += OnFinishElementAdd;
        }
        private void OnDisable()
        {
            startProp.drawHeaderCallback -= OnStartDrawHeader;
            startProp.drawElementCallback -= OnStartDrawElement;
            startProp.onAddCallback -= OnStartElementAdd;

            middleProp.drawHeaderCallback -= OnMiddleDrawHeader;
            middleProp.drawElementCallback -= OnMiddleDrawElement;
            middleProp.onAddCallback -= OnMiddleElementAdd;

            finishProp.drawHeaderCallback -= OnFinishDrawHeader;
            finishProp.drawElementCallback -= OnFinishDrawElement;
            finishProp.onAddCallback -= OnFinishElementAdd;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            startProp.DoLayoutList();

            EditorGUILayout.Space();
            middleProp.DoLayoutList();

            EditorGUILayout.Space();
            finishProp.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        /// ======================================================================

        private void OnStartDrawHeader(Rect rect)
        {
            DrawHeader(rect, startHeaderContent);
        }
        private void OnMiddleDrawHeader(Rect rect)
        {
            DrawHeader(rect, middleHeaderContent);
        }
        private void OnFinishDrawHeader(Rect rect)
        {
            DrawHeader(rect, finishHeaderContent);
        }

        private void OnStartDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            DrawElement(startProp, rect, index, isActive, isFocused, true);
        }
        private void OnMiddleDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            DrawElement(middleProp, rect, index, isActive, isFocused, false);
        }
        private void OnFinishDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            DrawElement(finishProp, rect, index, isActive, isFocused, true);
        }

        private void OnStartElementAdd(ReorderableList list)
        {
            AddElement(list);
            SetRequired(list, true);
            SetMirror(list, false);
        }
        private void OnMiddleElementAdd(ReorderableList list)
        {
            AddElement(list);
            SetRequired(list, false);
            SetMirror(list, false);
        }
        private void OnFinishElementAdd(ReorderableList list)
        {
            AddElement(list);
            SetRequired(list, true);
            SetMirror(list, false);
        }

        /// ======================================================================

        private static void DrawHeader(Rect rect, GUIContent content)
        {
            EditorGUI.LabelField(rect, content);
        }
        private static void DrawElement(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused, bool isDisableRequiredField)
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);

            var area = new Rect(
                rect.x + rect.width * bulderOffset,
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

            area = new Rect(
               rect.x + rect.width * mirrorOffset,
               rect.y + elementTopPadding,
               rect.width * mirrorScale,
               EditorGUIUtility.singleLineHeight);

            prop = element.FindPropertyRelative(isHorizontalMirrorField);
            prop.boolValue = EditorGUI.ToggleLeft(area, isHorizontalMirrorContent, prop.boolValue);

            if (isDisableRequiredField)
                GUI.enabled = false;

            area = new Rect(
                 rect.x + rect.width * requiredOffset,
                 rect.y + elementTopPadding,
                 rect.width * requiredScale,
                 EditorGUIUtility.singleLineHeight);

            prop = element.FindPropertyRelative(isRequiredField);
            prop.boolValue = EditorGUI.ToggleLeft(area, isRequiredContent, prop.boolValue);

            if (isDisableRequiredField)
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
        private static void SetMirror(ReorderableList list, bool value)
        {
            var index = list.serializedProperty.arraySize - 1;
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative(isHorizontalMirrorField).boolValue = value;
        }

        /// ======================================================================
    }
}