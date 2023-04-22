using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using ModularBuildingsFramework.Boxes.Standard;

namespace ModularBuildingsFramework.Boxes
{
	public class StandardBoxBuilding : ScriptableObject, IBoxBuilder
    {
        /// ======================================================================

        [SerializeField] private SideItemsCollection _sides;
        [SerializeField] private CapItem _cap;

        /// ======================================================================

        public float length
        {
            get { return SelectMaxLength(this); }
        }
        public float height
        {
            get { return SelectMaxHeight(this); }
        }
        public float depth
        {
            get { return SelectMaxDepth(this); }
        }

        private SideItemsCollection sides
        {
            get { return _sides; }
        }
        private CapItem cap
        {
            get { return _cap; }
        }

        private SideItem front
        {
            get { return sides.GetValidItem(0); }
        }
        private SideItem right
        {
            get { return sides.GetValidItem(1); }
        }
        private SideItem back
        {
            get { return sides.GetValidItem(2); }
        }
        private SideItem left
        {
            get { return sides.GetValidItem(3); }
        }

        /// ======================================================================

        public bool IsValid()
        {
            return Utils.IsValid(sides);
        }
        public void Reset()
        {
            _sides = SideItemsCollection.Create();
            _cap = CapItem.Create();
        }

        public void Build(BoxDraft draft)
        {
            if (!IsValid())
                return;

            if (!IsDraftValid(draft))
                return;

            var boxSettings = CreateBoxSettings(this, draft);
            if (!Utils.IsValid(boxSettings))
                return;

            // Front
            {
                var settings = CreateSideSettings(boxSettings);
                settings.length = boxSettings.length;
                settings.height = boxSettings.sidesHeight;
                settings.forwardScale = boxSettings.forwardScale;

                if (Utils.IsValid(settings))
                    GenerateLine(front, settings);
            }

            // Right
            {
                var settings = CreateSideSettings(boxSettings);
                settings.length = boxSettings.depth;
                settings.height = boxSettings.sidesHeight;
                settings.forwardScale = boxSettings.rightScale;
                settings.pivot = boxSettings.GetRightPivot();
                settings.right = boxSettings.GetRotatedRightAxis(-90f);

                if (Utils.IsValid(settings))
                    GenerateLine(right, settings);
            }

            // Back
            {
                var settings = CreateSideSettings(boxSettings);
                settings.length = boxSettings.length;
                settings.height = boxSettings.sidesHeight;
                settings.forwardScale = boxSettings.forwardScale;
                settings.pivot = boxSettings.GetBackPivot();
                settings.right = boxSettings.GetRotatedRightAxis(-180f);

                if (Utils.IsValid(settings))
                    GenerateLine(back, settings);
            }

            // Left
            {
                var settings = CreateSideSettings(boxSettings);
                settings.length = boxSettings.depth;
                settings.height = boxSettings.sidesHeight;
                settings.forwardScale = boxSettings.rightScale;
                settings.pivot = boxSettings.GetLeftPivot();
                settings.right = boxSettings.GetRotatedRightAxis(-270f);

                if (Utils.IsValid(settings))
                    GenerateLine(left, settings);
            }

            // Roof
            {
                var settings = CreateCapSettings(boxSettings);
                var depthScale = boxSettings.forwardScale;
                var lengthScale = boxSettings.rightScale;

                settings.pivot = boxSettings.pivot + boxSettings.up * boxSettings.sidesHeight;

                settings.length -= (left.depth + right.depth) * lengthScale;
                settings.pivot += boxSettings.right * left.depth * lengthScale;

                settings.depth -= (front.depth + back.depth) * depthScale;
                settings.pivot += boxSettings.forward * front.depth * depthScale;

                if (Utils.IsValid(settings))
                    if (Utils.IsValid(cap))
                        GenerateRoof(cap, settings);
            }
        }
        private bool IsDraftValid(BoxDraft draft)
        {
            if (!Utils.IsValid(draft))
                return false;

            if (Utils.NegativeOrZero(draft.height))
                return false;

            return true;
        }

        public float CalculateLengthScale(float length)
        {
            return CalculateLengthScale(this, length);
        }
        public float CalculateHeightScale(float height)
        {
            return CalculateHeightScale(this, height);
        }

        /// ======================================================================

        private static BoxGenerationSettings CreateBoxSettings(StandardBoxBuilding builder, BoxDraft draft)
        {
            var result = BoxGenerationSettings.Create();

            result.length = draft.length;
            result.depth = draft.depth;

            var capHeight = Utils.IsValid(builder.cap) ? builder.cap.height : 0f;

            var wallScaledHeight = draft.height - capHeight;
            var scale = builder.CalculateHeightScale(wallScaledHeight);
            var roofScaledHeigh = capHeight * scale;

            var totalScaledHeight = wallScaledHeight + roofScaledHeigh;
            if (!Utils.ApproximatelyZero(totalScaledHeight))
                scale = draft.height / totalScaledHeight;
            else
                scale = 0f;

            result.capHeight = capHeight * scale;
            result.sidesHeight = draft.height - result.capHeight;

            result.rightScale = builder.front.CalculateLengthScale(result.length);
            result.forwardScale = builder.right.CalculateLengthScale(result.depth);

            result.parent = draft.parent;
            result.pivot = draft.pivot;

            result.up = draft.up;
            result.right = draft.right;
            result.forward = draft.forward;

            return result;
        }
        private static SideGenerationSettings CreateSideSettings(BoxGenerationSettings settings)
        {
            var result = SideGenerationSettings.Create();

            result.length = settings.length;
            result.height = settings.sidesHeight;

            result.forwardScale = 1f;

            result.parent = settings.parent;
            result.pivot = settings.pivot;

            result.up = settings.up;
            result.right = settings.right;
            result.forward = settings.forward;

            return result;
        }
        private static CapGenerationSettings CreateCapSettings(BoxGenerationSettings settings)
        {
            var result = CapGenerationSettings.Create();

            result.length = settings.length;
            result.height = settings.capHeight;
            result.depth = settings.depth;

            result.parent = settings.parent;
            result.pivot = settings.pivot;

            result.up = settings.up;
            result.right = settings.right;
            result.forward = settings.forward;

            return result;
        }

        private static void GenerateLine(SideItem item, SideGenerationSettings settings)
        {
            var lineDraft = CreateLineDraft(settings);
            item.builder.Build(lineDraft);
        }
        private static LineDraft CreateLineDraft(SideGenerationSettings settings)
        {
            var result = LineDraft.Create();

            result.length = settings.length;
            result.height = settings.height;

            result.forwardScale = settings.forwardScale;

            result.parent = settings.parent;
            result.pivot = settings.pivot;

            result.up = settings.up;
            result.right = settings.right;

            return result;
        }

        private static void GenerateRoof(CapItem item, CapGenerationSettings settings)
        {
            var roofDraft = CreateRoofDraft(settings);
            item.builder.Build(roofDraft);
        }
        private static BoxDraft CreateRoofDraft(CapGenerationSettings settings)
        {
            var result = BoxDraft.Create();

            result.length = settings.length;
            result.depth = settings.depth;
            result.height = settings.height;

            result.parent = settings.parent;
            result.pivot = settings.pivot;

            result.up = settings.up;
            result.right = settings.right;

            return result;
        }

        private static float SelectMaxLength(StandardBoxBuilding building)
        {
            if (!Utils.EachValid(building.front, building.back))
                return 0f;

            return Mathf.Max(
                building.front.length,
                building.back.length
                );
        }
        private static float SelectMaxHeight(StandardBoxBuilding building)
        {
            if (!Utils.EachValid(building.front, building.right, building.back, building.left))
                return 0f;

            return Mathf.Max(
                building.front.height,
                building.right.height,
                building.back.height,
                building.left.height
                );
        }
        private static float SelectMaxDepth(StandardBoxBuilding building)
        {
            if (!Utils.EachValid(building.right, building.left))
                return 0f;

            return Mathf.Max(
                building.right.length,
                building.left.length
                );
        }

        private static float CalculateLengthScale(StandardBoxBuilding building, float targetLength)
        {
            if (!Utils.IsValid(building))
                return 1f;

            var items = building.sides.GetValidItems();
            var array = new float[items.Count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = items[i].CalculateLengthScale(targetLength);
            }
            return Utils.GetAverage(array);
        }
        private static float CalculateHeightScale(StandardBoxBuilding building, float targetHeight)
        {
            if (!Utils.IsValid(building))
                return 1f;

            var items = building.sides.GetValidItems();
            var array = new float[items.Count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = items[i].CalculateHeightScale(targetHeight);
            }
            return Utils.GetAverage(array);
        }

        /// ======================================================================

        private StandardBoxBuilding()
        {
            Reset();
        }

        /// ======================================================================
    }
}