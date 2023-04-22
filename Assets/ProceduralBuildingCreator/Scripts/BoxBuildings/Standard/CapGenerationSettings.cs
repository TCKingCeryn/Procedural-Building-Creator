using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Boxes.Standard
{
	public struct CapGenerationSettings : IValidable
	{
        /// ======================================================================

        public float length;
        public float height;
        public float depth;

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

            if (Utils.NegativeOrZero(length, depth))
                return false;

            return true;
        }
        public void Reset()
        {
            length = 10f;
            height = 10f;
            depth = 10f;

            parent = default(Transform);
            pivot = Vector3.zero;

            up = Vector3.up;
            right = Vector3.right;
            forward = Vector3.forward;
        }

        /// ======================================================================

        public static CapGenerationSettings Create()
        {
            var result = new CapGenerationSettings();
            result.Reset();
            return result;
        }

        /// ======================================================================
    }
}