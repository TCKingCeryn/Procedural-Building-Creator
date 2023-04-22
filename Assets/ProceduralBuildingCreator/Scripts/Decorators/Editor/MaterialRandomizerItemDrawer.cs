using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEditor;

namespace ModularBuildingsFramework.Decorators.Materials
{
    [CustomPropertyDrawer(typeof(MaterialRandomizerItem))]
    public class MaterialRandomizerItemDrawer : PropertyDrawer
    {
        /// ======================================================================

        private const string materialField = "_material";
        private const string weightField = "_weight";

        private static readonly GUIContent materialContent = GUIContent.none;
        private static readonly GUIContent weightContent = GUIContent.none;

        private const float materialScale = 0.5f;
        private const float weightScale = 0.5f;

        /// ======================================================================

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 1;

            var materialRect = new Rect(
                position.x, 
                position.y, 
                position.width * materialScale, 
                position.height
                );

            EditorGUI.PropertyField(materialRect, property.FindPropertyRelative(materialField), materialContent);

            var weightRect = new Rect(
                position.x + position.width * materialScale, 
                position.y, 
                position.width * weightScale, 
                position.height
                );
         
            EditorGUI.PropertyField(weightRect, property.FindPropertyRelative(weightField), weightContent);

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        /// ======================================================================
    }
}