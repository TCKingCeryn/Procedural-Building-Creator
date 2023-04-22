using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using ModularBuildingsFramework.Meshes;

namespace ModularBuildingsFramework
{
	public static class MeshCombiner 
	{
        /// ======================================================================

        private const int maxVertexCount = 65535;

        /// ======================================================================

        public static void Generate(Transform source, Transform target)
        {
            if (source == null)
                return;

            if (target == null)
                return;

            var dic = GeMaterialstDictionary(source);
            var items = GetCombineItems(dic);
            GenerateGameObjects(items, target);
        }

        private static Dictionary<Material, List<CombineInstance>> GeMaterialstDictionary(Transform parent)
        {
            var result = new Dictionary<Material, List<CombineInstance>>();

            var filters = parent.GetComponentsInChildren<MeshFilter>();
            foreach (var filter in filters)
            {
                var renderer = filter.GetComponent<Renderer>();
                if (renderer == null)
                    continue;

                for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                {
                    var list = default(List<CombineInstance>);
                    if (result.ContainsKey(renderer.sharedMaterials[i]))
                    {
                        list = result[renderer.sharedMaterials[i]];
                    }
                    else
                    {
                        list = new List<CombineInstance>();
                        result.Add(renderer.sharedMaterials[i], list);
                    }

                    var instance = new CombineInstance();
                    instance.mesh = GetSubmesh(filter.sharedMesh, i);
                    instance.transform = parent.worldToLocalMatrix * filter.transform.localToWorldMatrix;
                    list.Add(instance);
                }
            }

            return result;
        }
        private static List<MeshCombineItem> GetCombineItems(Dictionary<Material, List<CombineInstance>> dic)
        {
            var result = new List<MeshCombineItem>();

            foreach (var kvp in dic)
            {
                var material = kvp.Key;
                var instances = kvp.Value;
                var list = GetCombineItems(material, instances);
                result.AddRange(list);
            }

            return result;
        }
        private static List<MeshCombineItem> GetCombineItems(Material material, List<CombineInstance> list)
        {
            var result = new List<MeshCombineItem>();

            var item = new MeshCombineItem(material);
            result.Add(item);

            var vertexCount = 0;
            foreach (var instance in list)
            {
                if (vertexCount + instance.mesh.vertexCount > maxVertexCount)
                {
                    item = new MeshCombineItem(material);
                    result.Add(item);
                    vertexCount = 0;
                }

                item.instances.Add(instance);
                vertexCount += instance.mesh.vertexCount;
            }

            return result;
        }
        private static void GenerateGameObjects(List<MeshCombineItem> items, Transform parent)
        {
            Utils.DeleteChilds(parent);

            foreach (var item in items)
            {
                var go = new GameObject(item.material.name);
                go.transform.parent = parent;
                go.transform.localPosition = Vector3.zero;
                go.transform.localEulerAngles = Vector3.zero;

                var renderer = go.AddComponent<MeshRenderer>();
                renderer.sharedMaterial = item.material;

                var mesh = new Mesh();
                mesh.name = item.material.name;
                mesh.CombineMeshes(item.instances.ToArray());

                var filter = go.AddComponent<MeshFilter>();
                filter.sharedMesh = mesh;
            }
        }

        private static Mesh GetSubmesh(Mesh source, int index)
        {
            var result = new Mesh();

            result.vertices = source.vertices;
            result.uv = source.uv;
            result.normals = source.normals;

            result.triangles = source.GetTriangles(index);
            return result;
        }

        /// ======================================================================
    }
}