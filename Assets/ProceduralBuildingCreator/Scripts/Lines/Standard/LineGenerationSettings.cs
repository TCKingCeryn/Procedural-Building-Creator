using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Lines.Standard
{
	public struct LineGenerationSettings : IValidable
	{
        /// ======================================================================

        public float length;
        public float height;

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

            forwardScale = 1f;

            parent = default(Transform);
            pivot = Vector3.zero;

            up = Vector3.up;
            right = Vector3.right;
            forward = Vector3.forward;
        }

        /// ======================================================================

        public static LineGenerationSettings Create()
        {
            var result = new LineGenerationSettings();
            result.Reset();
            return result;
        }

        /// ======================================================================
    }
}