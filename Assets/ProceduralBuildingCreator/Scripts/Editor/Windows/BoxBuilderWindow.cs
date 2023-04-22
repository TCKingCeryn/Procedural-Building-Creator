using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEditor;

namespace ModularBuildingsFramework.Boxes
{
	public class BoxBuilderWindow : EditorWindow
	{
        /// ======================================================================

        private Object builder = default(Object);
        private BoxDraft draft = BoxDraft.Create();

        private bool isAutoBuild = false;
        private bool isRunDecorators = true;

        /// ======================================================================

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.Space();
            builder = EditorGUILayout.ObjectField("Builder:", builder, typeof(IBoxBuilder), false);
            draft.parent = (Transform)EditorGUILayout.ObjectField("Target:", draft.parent, typeof(Transform), true);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Draft", EditorStyles.centeredGreyMiniLabel);
            draft.length = EditorGUILayout.FloatField("Length:", draft.length);
            draft.height = EditorGUILayout.FloatField("Height:", draft.height);
            draft.depth = EditorGUILayout.FloatField("Depth:", draft.depth);

            draft.length = Mathf.Clamp(draft.length, 0f, draft.length);
            draft.height = Mathf.Clamp(draft.height, 0f, draft.height);
            draft.depth = Mathf.Clamp(draft.depth, 0f, draft.depth);

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

            var boxDraft = BoxDraft.Create(draft);
            boxDraft.Parse(draft.parent);
            (builder as IBoxBuilder).Build(boxDraft);

            if (isRunDecorators)
                Utils.RunDecorators(boxDraft.parent);
        }

        private void ResetDraft()
        {
            var parent = draft.parent;
            draft.Reset();
            draft.Parse(parent);

            var boxBuilder = builder as IBoxBuilder;
            if (boxBuilder != null)
            {
                draft.length = boxBuilder.length;
                draft.height = boxBuilder.height;
                draft.depth = boxBuilder.depth;
            }
        }
        private void ResetMisc()
        {
            isRunDecorators = true;
        }

        /// ======================================================================
    }
}