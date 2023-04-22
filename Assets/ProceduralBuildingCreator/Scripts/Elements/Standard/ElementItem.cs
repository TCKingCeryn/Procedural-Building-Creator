using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Elements.Standard
{
    // DONE
    [Serializable]
    public class ElementItem : IValidable, IRandomizable<ElementItem>
	{
        /// ======================================================================

        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _weight;

        /// ======================================================================

        public GameObject prefab
        {
            get { return _prefab; }
        }
        public int weight
        {
            get { return _weight; }
        }

        /// ======================================================================

        public bool IsValid()
        {
            if (prefab == null)
                return false;

            if (weight < 0f)
                return false;

            return true;
        }
        public void Reset()
        {
            _prefab = default(GameObject);
            _weight = 100;
        }

        /// ======================================================================

        private ElementItem()
        {
            Reset();
        }

        /// ======================================================================
    }
}