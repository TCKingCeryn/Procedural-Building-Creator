using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework
{
    [Serializable]
	public struct BoxDraft : IValidable
	{
        /// ======================================================================

        [SerializeField] private float _length;
        [SerializeField] private float _height;
        [SerializeField] private float _depth;

        [SerializeField] private Transform _parent;
        [SerializeField] private Vector3 _pivot;

        [SerializeField] private Vector3 _up;
        [SerializeField] private Vector3 _right;

        /// ======================================================================

        public float length
        {
            get { return _length; }
            set { _length = value; }
        }
        public float height
        {
            get { return _height; }
            set { _height = value; }
        }
        public float depth
        {
            get { return _depth; }
            set { _depth = value; }
        }

        public Transform parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        public Vector3 pivot
        {
            get { return _pivot; }
            set { _pivot = value; }
        }

        public Vector3 up
        {
            get { return _up; }
            set { _up = value.normalized; }
        }
        public Vector3 right
        {
            get { return _right; }
            set { _right = value.normalized; }
        }
        public Vector3 forward
        {
            get { return Vector3.Cross(right, up); }
        }

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
        }

        public void Copy(BoxDraft draft)
        {
            length = draft.length;
            height = draft.height;
            depth = draft.depth;

            parent = draft.parent;
            pivot = draft.pivot;

            up = draft.up;
            right = draft.right;
        }
        public void Parse(Transform transform)
        {
            if (transform == null)
                return;

            parent = transform;
            pivot = transform.position;

            up = transform.up;
            right = transform.right;
        }
       
        /// ======================================================================

        public static BoxDraft Create()
        {
            var result = new BoxDraft();
            result.Reset();
            return result;
        }
        public static BoxDraft Create(BoxDraft draft)
        {
            var result = Create();
            result.Copy(draft);
            return result;
        }
        public static BoxDraft Create(Transform transform)
        {
            var result = Create();
            result.Parse(transform);
            return result;
        }

        /// ======================================================================
    }
}