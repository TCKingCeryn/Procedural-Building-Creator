using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Demo
{
	public class CameraController : MonoBehaviour
	{
        /// ======================================================================

        public Transform target;

        public float distance = 10f;
        public float height = 5;
        public float heightDamping = 2;
        public float rotationDumping = 3;
        public float angleOffset = 0;

        [Space]
        public bool isUseMouseDrag;
        public float dragSpeed = 10;
        public float zoomSpeed = 5;
        public bool isInverse;

        private Vector3 dragStart;
        private float dragStartOffset;
        private float dragStartHeight;

        private const float dragHorizontalFactor = 1000f;
        private const float dragVerticalFactor = 100f;
        private const float zoomFactor = 100f;

        /// ======================================================================

        private void LateUpdate()
        {
            if (target == null) return;
            if (isUseMouseDrag) UpdateDrag();
            UpdatePosition();
        }

        /// ======================================================================

        private void UpdateDrag()
        {
            if (Input.GetMouseButtonDown(2))
            {
                dragStart = Input.mousePosition;
                dragStartOffset = angleOffset;
                dragStartHeight = height;
            }

            if (Input.GetMouseButton(2))
            {
                var mouseDelta = Input.mousePosition - dragStart;
                if (isInverse == false) mouseDelta *= -1f;

                var horDelta = Vector3.Cross(Vector3.up, mouseDelta);
                var normHorDelta = horDelta.z / Screen.width;
                angleOffset = dragStartOffset + dragSpeed * dragHorizontalFactor * normHorDelta * Time.deltaTime;

                var vertDelta = Vector3.Cross(Vector3.right, mouseDelta);
                var normVertDelta = vertDelta.z / Screen.height;
                height = dragStartHeight + dragSpeed * dragVerticalFactor * normVertDelta * Time.deltaTime;
            }

            var distanceDelta = Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed * zoomFactor;
            distance += distanceDelta * Time.deltaTime;

            distance = Mathf.Clamp(distance, 1f, 500f);
        }
        private void UpdatePosition()
        {
            var wantedRotationAngle = target.eulerAngles.y + angleOffset;
            var wantedHeight = target.position.y + height;

            var currentRotationAngle = transform.eulerAngles.y;
            var currentHeight = transform.position.y;

            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDumping * Time.deltaTime);
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            transform.position = target.position;
            transform.position -= Quaternion.Euler(0, currentRotationAngle, 0) * Vector3.forward * distance;

            var position = transform.position;
            position.y = currentHeight;
            transform.position = position;

            transform.LookAt(target);
        }

        /// ======================================================================
    }
}