using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Boxes.Standard
{
    [Serializable]
	public class CapItem : IValidable, ILength, IHeight, IDepth
	{
        /// ======================================================================

        [SerializeField] private Object _builder;

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

        public IBoxBuilder builder
        {
            get { return _builder as IBoxBuilder; }
        }

        /// ======================================================================

        public bool IsValid()
        {
            if (Utils.NegativeOrZero(length, depth))
                return false;

            return Utils.IsValid(builder);
        }
        public void Reset()
        {
            _builder = default(Object);
        }

        /// ======================================================================

        private CapItem()
        {
            Reset();
        }
        public static CapItem Create()
        {
            return new CapItem();
        }

        /// ======================================================================
    }
}