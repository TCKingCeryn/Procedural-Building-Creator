using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace ModularBuildingsFramework.Boxes
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(StandardBoxBuilding))]
    public class StandardBoxBuildingEditor : Editor
	{
        /// ======================================================================

        private ReorderableList sides;
        private SerializedProperty cap;

        private const string sidesCollectionField = "_sides";
        private const string sidesItemsField = "_items";
        private const string sidesBuilderField = "_builder";

        private const string capField = "_cap";
        private const string capBuilderField = "_builder";

        private static readonly GUIContent sidesHeaderContent = new GUIContent("Sides");
        private static readonly GUIContent sidesBuilderContent = new GUIContent("Side (ILineBuilder):");
        private static readonly GUIContent capBuilderContent = new GUIContent("Cap (IBoxBuilder):");

        private static readonly Type sidesBuilderType = typeof(ILineBuilder);
        private static readonly Type capBuilderType = typeof(IBoxBuilder);

        private const int elementTopPadding = 2;

        /// ======================================================================

        private void OnEnable()
        {
            var collection = serializedObject.FindProperty(sidesCollectionField);
            sides = new ReorderableList(
                serializedObject,
                collection.FindPropertyRelative(sidesItemsField),
                true,
                true,
                true,
                true
                );

            cap = serializedObject
                .FindProperty(capField)
                .FindPropertyRelative(capBuilderField);

            sides.drawHeaderCallback += OnSidesDrawHeader;
            sides.drawElementCallback += OnSidesDrawElement;
            sides.onAddCallback += OnSidesAddElement;
        }
        private void OnDisable()
        {
            sides.drawHeaderCallback -= OnSidesDrawHeader;
            sides.drawElementCallback -= OnSidesDrawElement;
            sides.onAddCallback -= OnSidesAddElement;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            sides.DoLayoutList();

            EditorGUILayout.Space();
            DrawCapField();

            serializedObject.ApplyModifiedProperties();
        }

        /// ======================================================================

        private void OnSidesDrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, sidesHeaderContent);
        }
        private void OnSidesDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = sides.serializedProperty.GetArrayElementAtIndex(index);

            var area = new Rect(
               rect.x,
               rect.y + elementTopPadding,
               rect.width,
               EditorGUIUtility.singleLineHeight);
            
            var prop = element.FindPropertyRelative(sidesBuilderField);
            prop.objectReferenceValue = EditorGUI.ObjectField(
                area,
                sidesBuilderContent,
                prop.objectReferenceValue,
                sidesBuilderType,
                false
                );
        }
        private void OnSidesAddElement(ReorderableList list)
        {
            list.serializedProperty.arraySize++;
        }

        private void DrawCapField()
        {
            cap.objectReferenceValue = EditorGUILayout.ObjectField(
                capBuilderContent,
                cap.objectReferenceValue,
                capBuilderType,
                false
                );

            if (ReferenceEquals(cap.objectReferenceValue, serializedObject.targetObject))
                cap.objectReferenceValue = null;
        }
    
        /// ======================================================================
    }
}
