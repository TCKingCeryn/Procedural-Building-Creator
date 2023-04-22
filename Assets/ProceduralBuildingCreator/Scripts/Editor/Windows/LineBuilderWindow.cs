using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEditor;

namespace ModularBuildingsFramework.Lines
{
	public class LineBuilderWindow : EditorWindow
    {
        /// ======================================================================

        private Object builder = default(Object);
        private LineDraft draft = LineDraft.Create();

        private bool isAutoBuild = false;
        private bool isRunDecorators = true;

        /// ======================================================================

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.Space();
            builder = EditorGUILayout.ObjectField("Builder:", builder, typeof(ILineBuilder), false);
            draft.parent = (Transform)EditorGUILayout.ObjectField("Parent:", draft.parent, typeof(Transform), true);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Draft", EditorStyles.centeredGreyMiniLabel);
            draft.length = EditorGUILayout.FloatField("Length:", draft.length);
            draft.height = EditorGUILayout.FloatField("Height:", draft.height);

            draft.length = Mathf.Clamp(draft.length, 0f, draft.length);
            draft.height = Mathf.Clamp(draft.height, 0f, draft.height);

            EditorGUILayout.Space();
            draft.forwardScale = EditorGUILayout.FloatField("Forward Scale:", draft.forwardScale);

            if (GUILayout.Button("Reset Draft"))
                ResetDraft();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Misc", EditorStyles.centeredGreyMiniLabel);
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
           
            var lineDraft = LineDraft.Create(draft);
            lineDraft.Parse(draft.parent);
            (builder as ILineBuilder).Build(lineDraft);

            if (isRunDecorators)
                Utils.RunDecorators(lineDraft.parent);
        }

        private void ResetDraft()
        {
            var parent = draft.parent;
            draft.Reset();
            draft.Parse(parent);

            var lineBuilder = builder as ILineBuilder;
            if (lineBuilder != null)
            {
                draft.length = lineBuilder.length;
                draft.height = lineBuilder.height;
            }
        }
        private void ResetMisc()
        {
            isRunDecorators = true;
        }

        /// ======================================================================
    }
}