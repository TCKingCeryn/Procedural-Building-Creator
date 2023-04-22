using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Lines.Standard
{
    [Serializable]
    public class LineItem : IValidable, IRequired, ILength, IHeight, IDepth
	{
        /// ======================================================================

        [SerializeField] private Object _builder;
        [SerializeField] private bool _isRequired;
        [SerializeField] private bool _isHorizontalMirror;

        /// ======================================================================
        
        public float length
        {
            get { return Utils.GetValidLength(builder); }
        }
        public float height
        {
            get { return Utils.GetValidHeight(builder); }
        }
        public float depth
        {
            get { return Utils.GetValidDepth(builder); }
        }

        public IColumnBuilder builder
        {
            get { return _builder as IColumnBuilder; }
        }
        public bool isRequired
        {
            get { return _isRequired; }
        }
        public bool isHorizontalMirror
        {
            get { return _isHorizontalMirror; }
        }

        /// ======================================================================

        public bool IsValid()
        {
            if (Utils.NegativeOrZero(length, height))
                return false;

            return Utils.IsValid(builder);
        }
        public void Reset()
        {
            _builder = default(Object);
            _isRequired = false;
            _isHorizontalMirror = false;
        }

        /// ======================================================================

        private LineItem()
        {
            Reset();
        }

        /// ======================================================================
    }
}