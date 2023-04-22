using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Roofs.StandardFlatBox
{
    [Serializable]
    public struct RoofParameters : IValidable
    {
        /// ======================================================================

        [SerializeField] private float _length;
        [SerializeField] private float _depth;
        [SerializeField] private RoofMode _mode;

        /// ======================================================================

        public float length
        {
            get { return _length; }
        }
        public float depth
        {
            get { return _depth; }
        }
        public RoofMode mode
        {
            get { return _mode; }
        }

        /// ======================================================================

        public bool IsValid()
        {
            if (Utils.NegativeOrZero(length, depth))
                return false;

            return true;
        }
        public void Reset()
        {
            _length = 10f;
            _depth = 10f;
            _mode = RoofMode.Tile;
        }

        /// ======================================================================

        public static RoofParameters Create()
        {
            var result = new RoofParameters();
            result.Reset();
            return result;
        }

        /// ======================================================================
    }
}