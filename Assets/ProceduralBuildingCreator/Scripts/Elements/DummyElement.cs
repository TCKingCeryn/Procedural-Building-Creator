using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Elements
{
    // DONE
	public class DummyElement : ScriptableObject, IElementBuilder
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

        public void Build(ElementDraft draft)
        {
            return;
        }

        public float CalculateLengthScale(float length)
        {
            if (Utils.ApproximatelyZero(length, this.length))
                return 1f;

            return length / this.length;
        }
        public float CalculateHeightScale(float height)
        {
            if (Utils.ApproximatelyZero(height, this.height))
                return 1f;

            return height / this.height;
        }

        /// ======================================================================
    }
}