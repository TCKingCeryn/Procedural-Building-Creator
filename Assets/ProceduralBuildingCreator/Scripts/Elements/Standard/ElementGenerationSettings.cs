using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Elements.Standard
{
    // DONE
	public struct ElementGenerationSettings : IValidable
	{
        /// ======================================================================

        public Transform parent;
        public Vector3 position;
        public Vector3 angles;
        public Vector3 scale;
                
        /// ======================================================================

        public bool IsValid()
        {
            if (parent == null)
                return false;

            if (Utils.ApproximatelyZero(scale.x, scale.y, scale.z))
                return false;

            return true;
        }
        public void Reset()
        {
            parent = default(Transform);
            position = Vector3.zero;
            angles = Vector3.zero;
            scale = Vector3.one;
        }

        /// ======================================================================

        public static ElementGenerationSettings Create()
        {
            var result = new ElementGenerationSettings();
            result.Reset();
            return result;
        }

        /// ======================================================================
    }
}