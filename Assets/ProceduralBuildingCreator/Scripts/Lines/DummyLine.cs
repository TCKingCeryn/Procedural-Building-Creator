using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Lines
{
    public class DummyLine : ScriptableObject, ILineBuilder
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
            if(Utils.Negative(length, height, depth))
                return false;

            return true;
        }
        public void Reset()
        {
            _length = 10f;
            _height = 10f;
            _depth = 10f;
        }

        public void Build(LineDraft draft)
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

        private static float CalculateLengthScale(DummyLine line, float length)
        {
            if (Utils.ApproximatelyZero(length, line.length))
                return 1f;

            return length / line.length;
        }
        private static float CalculateHeightScale(DummyLine line, float height)
        {
            if (Utils.ApproximatelyZero(height, line.height))
                return 1f;

            return height / line.height;
        }

        /// ======================================================================

        private DummyLine()
        {
            Reset();
        }

        /// ======================================================================
    }
}