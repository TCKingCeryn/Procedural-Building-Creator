using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Boxes
{
    public class DummyBoxBuilder : ScriptableObject, IBoxBuilder
    {
        /// ======================================================================

        [SerializeField] private float _length;
        [SerializeField] private float _height;
        [SerializeField] private float _depth;

        /// ======================================================================

        public float length
        {
            get { return _length; }
        }
        public float height
        {
            get { return _height; }
        }
        public float depth
        {
            get { return _depth; }
        }

        /// ======================================================================

        public bool IsValid()
        {
            if (Utils.Negative(length, height, depth))
                return false;

            return true;
        }
        public void Reset()
        {
            _length = 10f;
            _height = 10f;
            _depth = 10f;
        }

        public void Build(BoxDraft draft)
        {
            return;
        }

        public float CalculateLengthScale(float length)
        {
            return CalculateLengthScale(this, length);
        }
        public float CalculateHeightScale(float height)
        {
            return CalculateHeightScale(this, height);
        }

        /// ======================================================================

        private static float CalculateLengthScale(DummyBoxBuilder builder, float length)
        {
            if (Utils.ApproximatelyZero(length, builder.length))
                return 1f;

            return length / builder.length;
        }
        private static float CalculateHeightScale(DummyBoxBuilder builder, float height)
        {
            if (Utils.ApproximatelyZero(height, builder.height))
                return 1f;

            return height / builder.height;
        }

        /// ======================================================================

        private DummyBoxBuilder()
        {
            Reset();
        }

        /// ======================================================================
    }
}