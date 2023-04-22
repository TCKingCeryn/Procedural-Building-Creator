using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using ModularBuildingsFramework.Elements.Standard;

namespace ModularBuildingsFramework.Elements
{
    // DONE
	public class StandardElement : ScriptableObject, IElementBuilder
	{
        /// ======================================================================

        [SerializeField] private ElementParameters _parameters;
        [SerializeField] private ElementItemsCollection _items;

        /// ======================================================================

        public float length
        {
            get { return parameters.length; }
        }
        public float height
        {
            get { return parameters.height; }
        }
        public float depth
        {
            get { return parameters.depth; }
        }

        private ElementParameters parameters
        {
            get { return _parameters; }
        }
        private ElementItemsCollection items
        {
            get { return _items; }
        }

        /// ======================================================================

        public bool IsValid()
        {
            if (!Utils.IsValid(parameters))
                return false;

            return Utils.IsValid(items);
        }
        public void Reset()
        {
            _parameters = ElementParameters.Create();
            _items = ElementItemsCollection.Create();
        }

        public void Build(ElementDraft draft)
        {
            if (!Utils.IsValid(this))
                return;

            if (!Utils.IsValid(draft))
                return;

            var item = items.GetRandomValidItem();
            if (!Utils.IsValid(item))
                return;

            var settings = CreateSettings(parameters, draft);
            if (!Utils.IsValid(settings))
                return;

            GenerateItem(item, settings);
        }

        public float CalculateLengthScale(float length)
        {
            if (!Utils.ApproximatelyZero(length, this.length))
                return 1f;

            return length / this.length;
        }
        public float CalculateHeightScale(float height)
        {
            if (!Utils.ApproximatelyZero(height, this.height))
                return 1f;

            return height / this.height;
        }

        /// ======================================================================

        private static ElementGenerationSettings CreateSettings(ElementParameters parameters, ElementDraft draft)
        {
            var result = ElementGenerationSettings.Create();

            var position = draft.pivot;
            var angles = Quaternion.LookRotation(draft.forward, draft.up).eulerAngles;
            var scale = Vector3.one;

            var isHorizontalMrror = parameters.isHorizontalMrror;
            if (draft.isHorizontalMrror)
                isHorizontalMrror = !isHorizontalMrror;

            var isVerticalMirror = parameters.isVerticalMirror;
            if (draft.isVerticalMirror)
                isVerticalMirror = !isVerticalMirror;
            
            if (Utils.ApproximatelyZero(parameters.length))
                scale.x = 1f;
            else if (Utils.ApproximatelyZero(draft.length))
                scale.x = 0f;
            else
                scale.x = draft.length / parameters.length;

            if (Utils.ApproximatelyZero(parameters.height))
                scale.y = 1f;
            else if (Utils.ApproximatelyZero(draft.height))
                scale.y = 0f;
            else
                scale.y = draft.height / parameters.height;

            if (Utils.ApproximatelyZero(draft.forwardScale))
                scale.z = 1f;
            else
                scale.z = draft.forwardScale;

            if (isHorizontalMrror)
            {
                scale.x *= -1f;
                position -= draft.right * parameters.length * scale.x;
            }

            if (isVerticalMirror)
            {
                scale.y *= -1f;
                position -= draft.up * parameters.height * scale.y;
            }

            result.parent = draft.parent;
            result.position = position;
            result.angles = angles;
            result.scale = scale;

            return result;
        }
        private static void GenerateItem(ElementItem item, ElementGenerationSettings settings)
        {
            var go = Utils.Instantiate(item.prefab);
            if (go == null)
                return;

            go.transform.parent = settings.parent;
            go.transform.position = settings.position;
            go.transform.eulerAngles = settings.angles;
            go.transform.localScale = settings.scale;
        }

        /// ======================================================================

        private StandardElement()
        {
            Reset();
        }

        /// ======================================================================
    }
}