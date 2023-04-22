using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Roofs.StandardFlatBox
{
	public struct RoofGenerationSettings : IValidable
	{
        /// ======================================================================

        public float itemLength;
        public float itemDepth;

        public float targetLength;
        public float targetDepth;

        public Transform parent;
        public Vector3 pivot;

        public Vector3 up;
        public Vector3 right;
        public Vector3 forward;

        /// ======================================================================

        public bool IsValid()
        {
            if (Utils.NegativeOrZero(itemLength, itemDepth, targetLength, targetDepth))
                return false;

            return true;
        }
        public void Reset()
        {
            itemLength = 10f;
            itemDepth = 10f;

            targetLength = 10f;
            targetDepth = 10f;

            parent = default(Transform);
            pivot = Vector3.zero;

            up = Vector3.up;
            right = Vector3.right;
            forward = Vector3.forward;
        }

        /// ======================================================================

        public static RoofGenerationSettings Create()
        {
            var result = new RoofGenerationSettings();
            result.Reset();
            return result;
        }

        /// ======================================================================
    }
}