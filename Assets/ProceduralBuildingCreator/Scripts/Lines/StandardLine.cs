using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using ModularBuildingsFramework.Lines.Standard;

namespace ModularBuildingsFramework.Lines
{
	public class StandardLine : ScriptableObject, ILineBuilder
	{
        /// ======================================================================

        [SerializeField] private LineItemsCollection _start;
        [SerializeField] private LineItemsCollection _middle;
        [SerializeField] private LineItemsCollection _finish;
        
        /// ======================================================================

        public float length
        {
            get { return GetTotalMinLength(this); }
        }
        public float height
        {
            get { return SelectMaxHeight(this); }
        }
        public float depth
        {
            get { return SelectMaxDepth(this); }
        }

        private LineItemsCollection start
        {
            get { return _start; }
        }
        private LineItemsCollection middle
        {
            get { return _middle; }
        }
        private LineItemsCollection finish
        {
            get { return _finish; }
        }

        /// ======================================================================

        public bool IsValid()
        {
            return Utils.AnyValid(start, middle, finish);
        }
        public void Reset()
        {
            _start = LineItemsCollection.Create();
            _middle = LineItemsCollection.Create();
            _finish = LineItemsCollection.Create();
        }

        public void Build(LineDraft draft)
        {
            if (!Utils.IsValid(this))
                return;

            if (!Utils.IsValid(draft))
                return;

            var settings = CreateSettings(this, draft);
            if (!Utils.IsValid(settings))
                return;

            var list = CalculateItems(this, settings.length);
            if (!Utils.NotNullAndHasElements(list))
                return;

            GenerateItems(list, settings);
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

        private static LineGenerationSettings CreateSettings(StandardLine builder, LineDraft draft)
        {
            var result = LineGenerationSettings.Create();

            result.length = draft.length;
            result.height = draft.height;

            result.forwardScale = draft.forwardScale;

            result.parent = draft.parent;
            result.pivot = draft.pivot;

            result.up = draft.up;
            result.right = draft.right;
            result.forward = draft.forward;

            return result;
        }
        private static List<LineItem> CalculateItems(StandardLine builder, float length)
        {
            var totalMinLength = GetTotalMinLength(builder);
            if (length < totalMinLength)
            {
                var startItems = builder.start.GetRequiredItems();
                var middleItems = builder.middle.GetRequiredItems();
                var finishItems = builder.finish.GetRequiredItems();
                return Utils.Combine(startItems, middleItems, finishItems);
            }
            else
            {
                var startItems = builder.start.GetValidItems();
                var finishItems = builder.finish.GetValidItems();

                var edgesLength = 0f;
                edgesLength += Utils.CalculateLengthSum(startItems);
                edgesLength += Utils.CalculateLengthSum(finishItems);
                var middleLength = length - edgesLength;

                var items = builder.middle.GetValidItems();
                var count = Utils.CalculateItemsCountByLength(items, middleLength);
                var middleItems = Utils.FillRepeated(items, count);

                return Utils.Combine(startItems, middleItems, finishItems);
            }
        }

        private static void GenerateItems(List<LineItem> list, LineGenerationSettings settings)
        {
            var totalLength = Utils.CalculateLengthSum(list);
            var lengthScale = settings.length / totalLength;

            var columnDraft = CreateColumnDraft(settings);
            foreach (var item in list)
            {
                columnDraft.isHorizontalMrror = item.isHorizontalMirror;
                columnDraft.length = item.builder.length * lengthScale;

                item.builder.Build(columnDraft);
                columnDraft.pivot += columnDraft.right * columnDraft.length;
            }
        }
        private static ColumnDraft CreateColumnDraft(LineGenerationSettings settings)
        {
            var result = ColumnDraft.Create();

            result.length = settings.length;
            result.height = settings.height;

            result.isHorizontalMrror = false;
            result.forwardScale = settings.forwardScale;

            result.parent = settings.parent;
            result.pivot = settings.pivot;

            result.up = settings.up;
            result.right = settings.right;

            return result;
        }

        private static float GetTotalMinLength(StandardLine line)
        {
            var result = 0f;

            var items = line.start.GetRequiredItems();
            result += Utils.CalculateLengthSum(items);

            items = line.middle.GetRequiredItems();
            result += Utils.CalculateLengthSum(items);

            items = line.finish.GetRequiredItems();
            result += Utils.CalculateLengthSum(items);

            return result;
        }
        private static float SelectMaxHeight(StandardLine line)
        {
            var items = line.start.GetValidItems();
            var startMax = Utils.SelectMaxHeight(items);

            items = line.middle.GetRequiredItems();
            var middleMax = Utils.SelectMaxHeight(items);

            items = line.finish.GetValidItems();
            var finishMax = Utils.SelectMaxHeight(items);

            return Mathf.Max(startMax, middleMax, finishMax);
        }
        private static float SelectMaxDepth(StandardLine line)
        {
            var items = line.start.GetValidItems();
            var startMax = Utils.SelectMaxDepth(items);

            items = line.middle.GetValidItems();
            var middleMax = Utils.SelectMaxDepth(items);

            items = line.finish.GetValidItems();
            var finishMax = Utils.SelectMaxDepth(items);

            return Mathf.Max(startMax, middleMax, finishMax);
        }

        private static float CalculateLengthScale(StandardLine line, float length)
        {
            if (Utils.ApproximatelyZero(length))
                return 1f;

            var items = CalculateItems(line, length);
            var totalLength = Utils.CalculateLengthSum(items);
            return length / totalLength;
        }
        private static float CalculateHeightScale(StandardLine line, float height)
        {
            if (Utils.ApproximatelyZero(height))
                return 1f;

            var startScale = line.start.GetAvergeScaleByHeight(height);
            var middleScale = line.middle.GetAvergeScaleByHeight(height);
            var finishScale = line.finish.GetAvergeScaleByHeight(height);

            var result = Utils.GetAverage(startScale, middleScale, finishScale);
            if (Utils.ApproximatelyZero(result))
                return 1f;

            return result;
        }

        /// ======================================================================

        private StandardLine()
        {
            Reset();
        }

        /// ======================================================================
    }
}