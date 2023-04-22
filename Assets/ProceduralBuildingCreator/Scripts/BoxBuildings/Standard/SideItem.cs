using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Boxes.Standard
{
    [Serializable]
	public class SideItem : IValidable, ILength, IHeight, IDepth
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

        public ILineBuilder builder
        {
            get { return _builder as ILineBuilder; }
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
        }

        public float CalculateLengthScale(float length)
        {
            return CalculateLengthScale(builder, length);
        }
        public float CalculateHeightScale(float height)
        {
            return CalculateHeightScale(builder, height);
        }

        /// ======================================================================

        private static float CalculateLengthScale(ILineBuilder builder, float length)
        {
            if (!Utils.IsValid(builder))
                return 1f;

            return builder.CalculateLengthScale(length);
        }
        private static float CalculateHeightScale(ILineBuilder builder, float height)
        {
            if (!Utils.IsValid(builder))
                return 1f;

            return builder.CalculateHeightScale(height);
        }

        /// ======================================================================

        private SideItem()
        {
            Reset();
        }

        /// ======================================================================
    }
}