using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Decorators.Materials
{
    [Serializable]
	public class MaterialRandomizerItem : IValidable, IRandomizable<MaterialRandomizerItem>
	{
        /// ======================================================================

        [SerializeField] private Material _material;
        [Range(0, 100)]
        [SerializeField] private int _weight;

        /// ======================================================================

        public Material material
        {
            get { return _material; }
        }
        public int weight
        {
            get { return _weight; }
        }

        /// ======================================================================

        public bool IsValid()
        {
            if (material == null)
                return false;

            if (weight < 0)
                return false;

            return true;
        }
        public void Reset()
        {
            _material = default(Material);
            _weight = 100;
        }

        /// ======================================================================

        public MaterialRandomizerItem()
        {
            Reset();
        }

        /// ======================================================================
    }
}