using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEditor;

namespace ModularBuildingsFramework
{
	public class MeshCombinerWindow : EditorWindow
    {
        /// ======================================================================

        private Transform source;
        private Transform target;

        /// ======================================================================

        private void OnGUI()
        {
            EditorGUILayout.Space();
            source = (Transform)EditorGUILayout.ObjectField("Source:", source, typeof(Transform), true);
            target = (Transform)EditorGUILayout.ObjectField("Target:", target, typeof(Transform), true);

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Export"))
                Export();
            if (GUILayout.Button("Combine"))
                Combine();
            EditorGUILayout.EndHorizontal();
        }

        private void Export()
        {
            if (target == null)
                return;

            var path = EditorUtility.SaveFilePanel(
                "Export Mesh",
                Application.dataPath,
                target.name,
                "asset");

            if (string.IsNullOrEmpty(path))
                return;

            path = path.Replace(Application.dataPath, "Assets");
            MeshExporter.Export(target, path);
        }
        private void Combine()
        {
            if (source == null)
                return;

            if (target == null)
                return;

            MeshCombiner.Generate(source, target);
        }

        /// ======================================================================
    }
}