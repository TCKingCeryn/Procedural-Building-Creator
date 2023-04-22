using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Boxes.Standard
{
	public struct BoxGenerationSettings : IValidable
	{
        /// ======================================================================

        public float length;
        public float depth;

        public float sidesHeight;
        public float capHeight;

        public float rightScale;
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

            if (Utils.NegativeOrZero(length, depth))
                return false;

            return true;
        }
        public void Reset()
        {
            length = 10f;
            depth = 10f;

            sidesHeight = 9f;
            capHeight = 1f;

            rightScale = 1f;
            forwardScale = 1f;

            parent = default(Transform);
            pivot = Vector3.zero;

            up = Vector3.up;
            right = Vector3.right;
            forward = Vector3.forward;
        }

        public Vector3 GetFrontPivot()
        {
            return pivot;
        }
        public Vector3 GetRightPivot()
        {
            return pivot + right * length;
        }
        public Vector3 GetBackPivot()
        {
            return pivot + right * length + forward * depth;
        }
        public Vector3 GetLeftPivot()
        {
            return pivot + forward * depth;
        }

        public Vector3 GetRotatedRightAxis(float angle)
        {
            return Quaternion.AngleAxis(angle, up) * right;
        }

        /// ======================================================================

        public static BoxGenerationSettings Create()
        {
            var result =  new BoxGenerationSettings();
            result.Reset();
            return result;
        }

        /// ======================================================================
    }
}