using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Columns.Standard
{
    // DONE
	public struct ColumnGenerationSettings : IValidable
	{
        /// ======================================================================

        public float length;
        public float height;

        public bool isHorizontalMrror;
        public float forwardScale;

        public Transform parent;
        public Vector3 pivot;

        public Vector3 up;
        public Vector3 right;
        public Vector3 forward;

        /// ======================================================================

        public bool IsValid()
        {
            if (parent == null)
                return false;

            if (Utils.NegativeOrZero(length, height))
                return false;

            return true;
        }
        public void Reset()
        {
            length = 10f;
            height = 10f;

            isHorizontalMrror = false;
            forwardScale = 1f;

            parent = default(Transform);
            pivot = Vector3.zero;

            up = Vector3.up;
            right = Vector3.right;
            forward = Vector3.forward;
        }

        /// ======================================================================

        public static ColumnGenerationSettings Create()
        {
            var result = new ColumnGenerationSettings();
            result.Reset();
            return result;
        }
 
        /// ======================================================================
    }
}