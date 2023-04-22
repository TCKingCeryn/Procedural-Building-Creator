using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Columns
{
    // DONE
	public class DummyColumn : ScriptableObject, IColumnBuilder
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

        public void Build(ColumnDraft draft)
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

        private static float CalculateLengthScale(DummyColumn column, float length)
        {
            if (Utils.ApproximatelyZero(length, column.length))
                return 1f;

            return length / column.length;
        }
        private static float CalculateHeightScale(DummyColumn column, float height)
        {
            if (Utils.ApproximatelyZero(height, column.height))
                return 1f;

            return height / column.height;
        }

        /// ======================================================================

        private DummyColumn()
        {
            Reset();
        }

        /// ======================================================================
    }
}