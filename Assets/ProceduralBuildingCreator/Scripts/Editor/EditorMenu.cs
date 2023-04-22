using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEditor;

namespace ModularBuildingsFramework
{
	public static class EditorMenu 
	{
        /// ======================================================================

        public const string rootPath = "Tools/Modular Buildings/";

        public const string windowsRoot = rootPath + "Builders/";
        public const string elementsRoot = rootPath + "Elements/";
        public const string utilityRoot = rootPath + "Utils/";

        public const int windowsPriority = 1;
        public const int elementsPriorioty = 2;
        public const int utilityPriority = 3;

        /// ======================================================================

        [MenuItem(elementsRoot  + "Buildings/Standard Box", priority = elementsPriorioty + 1)]
        private static void CreateStandardBox()
        {
            CreateAsset<Boxes.StandardBoxBuilding>("Stanard Box Building");
        }

        [MenuItem(elementsRoot + "Buildings/Dummy Box", priority = elementsPriorioty + 2)]
        private static void CreateDummyBox()
        {
            CreateAsset<Boxes.DummyBoxBuilder>("Dummy Box");
        }

        [MenuItem(elementsRoot + "Roofs/Standard Flat Box", priority = elementsPriorioty + 1)]
        private static void CreateStandardBoxRoofBuilder()
        {
            CreateAsset<Roofs.StandardFlatBoxRoof>("Stanard Flat Box Roof");
        }

        [MenuItem(elementsRoot + "Lines/Standard", priority = elementsPriorioty + 1)]
        private static void CreateStandardLineBuilder()
        {
            CreateAsset<Lines.StandardLine>("Stanard Line");
        }

        [MenuItem(elementsRoot + "Lines/Dummy", priority = elementsPriorioty + 2)]
        private static void CreateDummyLineBuilder()
        {
            CreateAsset<Lines.DummyLine>("Dummy Line");
        }

        [MenuItem(elementsRoot + "Columns/Standard", priority = elementsPriorioty + 1)]
        private static void CreateStandardColumnBuilder()
        {
            CreateAsset<Columns.StandardColumn>("Stanard Column");
        }

        [MenuItem(elementsRoot + "Columns/Dummy", priority = elementsPriorioty + 2)]
        private static void CreateDummyColumnBuilder()
        {
            CreateAsset<Columns.DummyColumn>("Dummy Column");
        }

        [MenuItem(elementsRoot + "Elements/Standard", priority = elementsPriorioty + 1)]
        private static void CreateStandardElementBuilder()
        {
            CreateAsset<Elements.StandardElement>("Stanard Element");
        }

        [MenuItem(elementsRoot + "Elements/Dummy", priority = elementsPriorioty + 2)]
        private static void CreateDummyElementBuilder()
        {
            CreateAsset<Elements.DummyElement>("Dummy Element");
        }

        /// ======================================================================

        [MenuItem(windowsRoot + "Box", priority = windowsPriority + 1)]
        private static void GetBoxBuilderWindow()
        {
            EditorWindow.GetWindow<Boxes.BoxBuilderWindow>("Box");
        }

        [MenuItem(windowsRoot + "Line", priority = windowsPriority + 2)]
        private static void GetLineBuilderWindow()
        {
            EditorWindow.GetWindow<Lines.LineBuilderWindow>("Line");
        }

        [MenuItem(windowsRoot + "Column", priority = windowsPriority + 3)]
        private static void GetColumnBuilderWindow()
        {
            EditorWindow.GetWindow<Columns.ColumnBuilderWindow>("Column");
        }

        [MenuItem(windowsRoot + "Element", priority = windowsPriority + 4)]
        private static void GetElementBuilderWindow()
        {
            EditorWindow.GetWindow<Elements.ElementBuilderWindow>("Element");
        }

        /// ======================================================================

        [MenuItem(utilityRoot + "Mesh Combiner", priority = utilityPriority + 9)]
        private static void GetMeshCombinerWindowWindow()
        {
            EditorWindow.GetWindow<MeshCombinerWindow>("Mesh Combiner");
        }

        /// ======================================================================

        private static void CreateAsset<T>(string name) where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();

            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + name + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        /// ======================================================================
    }
}