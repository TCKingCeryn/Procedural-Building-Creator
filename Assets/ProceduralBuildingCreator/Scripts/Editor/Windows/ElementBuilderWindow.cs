using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEditor;
using ModularBuildingsFramework.Elements;

namespace ModularBuildingsFramework.Elements
{
    // DONE
	public class ElementBuilderWindow : EditorWindow
    {
        /// ======================================================================

        private Object builder = default(Object);
        private ElementDraft draft = ElementDraft.Create();

        private int repeatRight = 1;
        private int repeatUp = 1;

        private bool isAutoBuild = false;
        private bool isRunDecorators = true;

        /// ======================================================================

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.Space();
            builder = EditorGUILayout.ObjectField("Builder:", builder as Object, typeof(IElementBuilder), false);
            draft.parent = (Transform)EditorGUILayout.ObjectField("Parent:", draft.parent, typeof(Transform), true);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Draft", EditorStyles.centeredGreyMiniLabel);
            draft.length = EditorGUILayout.FloatField("Length:", draft.length);
            draft.height = EditorGUILayout.FloatField("Height:", draft.height);
   
            draft.length = Mathf.Clamp(draft.length, 0f, draft.length);
            draft.height = Mathf.Clamp(draft.height, 0f, draft.height);

            EditorGUILayout.Space();
            draft.isHorizontalMrror = EditorGUILayout.Toggle("Horizontal Mrror: ", draft.isHorizontalMrror);
            draft.isVerticalMirror = EditorGUILayout.Toggle("Vertical Mirror: ", draft.isVerticalMirror);
            draft.forwardScale = EditorGUILayout.FloatField("Forward Scale:", draft.forwardScale);

            if (GUILayout.Button("Reset Draft"))
                ResetDraft();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Misc", EditorStyles.centeredGreyMiniLabel);
            repeatRight = EditorGUILayout.IntSlider("Repeat Right:", repeatRight, 1, 10);
            repeatUp = EditorGUILayout.IntSlider("Repeat Up:", repeatUp, 1, 10);
            isRunDecorators = EditorGUILayout.Toggle("Run Decorators", isRunDecorators);

            if (GUILayout.Button("Reset Misc"))
                ResetMisc();
           
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            isAutoBuild = EditorGUILayout.Toggle("Auto Build:", isAutoBuild);

            if (EditorGUI.EndChangeCheck() && isAutoBuild)
                Build();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear"))
                Clear();
            if (GUILayout.Button("Build"))
                Build();
            EditorGUILayout.EndHorizontal();
        }

        private void Clear()
        {
            if (draft.parent != null)
                Utils.DeleteChilds(draft.parent);
        }
        private void Build()
        {
            if (builder == null)
                return;

            if (draft.parent == null)
                return;

            Utils.DeleteChilds(draft.parent);

            for (int y = 0; y < repeatUp; y++)
            {
                for (int x = 0; x < repeatRight; x++)
                {
                    var elementDraft = ElementDraft.Create(draft);
                    elementDraft.Parse(draft.parent);

                    elementDraft.pivot = elementDraft.pivot
                        + elementDraft.right * elementDraft.length * x
                        + elementDraft.up * elementDraft.height * y;

                    (builder as IElementBuilder).Build(elementDraft);

                    if (isRunDecorators)
                        Utils.RunDecorators(elementDraft.parent);
                }
            }
        }

        private void ResetDraft()
        {
            var parent = draft.parent;
            draft.Reset();
            draft.Parse(parent);

            var elementBuilder = builder as IElementBuilder;
            if (elementBuilder != null)
            {
                draft.length = elementBuilder.length;
                draft.height = elementBuilder.height;
            }
        }
        private void ResetMisc()
        {
            repeatRight = 1;
            repeatUp = 1;
            isRunDecorators = true;
        }

        /// ======================================================================
    }
}