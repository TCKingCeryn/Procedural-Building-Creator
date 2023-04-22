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
	public struct ElementParameters : IValidable
	{
        /// ======================================================================

        [SerializeField] private float _length;
        [SerializeField] private float _height;
        [SerializeField] private float _depth;

        [SerializeField] private bool _isHorizontalMrror;
        [SerializeField] private bool _isVerticalMirror;

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

        public bool isHorizontalMrror
        {
            get { return _isHorizontalMrror; }
        }
        public bool isVerticalMirror
        {
            get { return _isVerticalMirror; }
        }

        /// ======================================================================

        public bool IsValid()
        {
            if (Utils.NegativeOrZero(length, height))
                return false;
            
            return true;
        }
        public void Reset()
        {
            _length = 10f;
            _height = 10f;
            _depth = 0;

            _isHorizontalMrror = false;
            _isVerticalMirror = false;
        }

        /// ======================================================================

        public static ElementParameters Create()
        {
            var result = new ElementParameters();
            result.Reset();
            return result;
        }

        /// ======================================================================
    }
}