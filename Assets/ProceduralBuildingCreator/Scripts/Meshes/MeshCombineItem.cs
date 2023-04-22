using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Meshes
{
	public class MeshCombineItem 
	{
        /// ======================================================================

        public readonly Material material;
        public readonly List<CombineInstance> instances;

        /// ======================================================================

        public MeshCombineItem(Material material)
        {
            this.material = material;
            instances = new List<CombineInstance>();
        }

        /// ======================================================================
    }
}