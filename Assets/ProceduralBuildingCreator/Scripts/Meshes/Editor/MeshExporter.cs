using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEditor;

namespace ModularBuildingsFramework
{
	public static class MeshExporter 
	{
        /// ======================================================================

        public static void Export(Transform parent, string path)
        {
            if (parent == null)
                return;

            if (string.IsNullOrEmpty(path))
                return;

            var asset = new Mesh();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.GetMainAssetTypeAtPath(path);

            var array = parent.GetComponentsInChildren<MeshFilter>();
            foreach (var filter in array)
            {
                if (filter.sharedMesh == null)
                    continue;

                var mesh = Object.Instantiate(filter.sharedMesh);
                mesh.name = mesh.name.Replace("(Clone)", "");
                filter.sharedMesh = mesh;

                AssetDatabase.AddObjectToAsset(mesh, asset);
            }

            AssetDatabase.ImportAsset(path);
        }

        /// ======================================================================
    }
}