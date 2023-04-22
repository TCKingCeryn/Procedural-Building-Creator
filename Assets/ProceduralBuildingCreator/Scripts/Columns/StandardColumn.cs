using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using ModularBuildingsFramework.Columns.Standard;

namespace ModularBuildingsFramework.Columns
{
    // DONE
	public class StandardColumn : ScriptableObject, IColumnBuilder
	{
        /// ======================================================================

        [SerializeField] private ColumnItemsCollection _bottom;
        [SerializeField] private ColumnItemsCollection _middle;
        [SerializeField] private ColumnItemsCollection _top;

        /// ======================================================================

        public float length
        {
            get { return SelectMaxLength(this); }
        }
        public float height
        {
            get { return GetTotalMinHeight(this); }
        }
        public float depth
        {
            get { return SelectMaxDepth(this); }
        }

        private ColumnItemsCollection bottom
        {
            get { return _bottom; }
        }
        private ColumnItemsCollection middle
        {
            get { return _middle; }
        }
        private ColumnItemsCollection top
        {
            get { return _top; }
        }

        /// ======================================================================

        public bool IsValid()
        {
            return Utils.AnyValid(bottom, middle, top);
        }
        public void Reset()
        {
            _bottom = ColumnItemsCollection.Create();
            _middle = ColumnItemsCollection.Create();
            _top = ColumnItemsCollection.Create();
        }

        public void Build(ColumnDraft draft)
        {
            if (!Utils.IsValid(this))
                return;

            if (!Utils.IsValid(draft))
                return;

            var settings = CreateSettings(draft);
            if (!Utils.IsValid(settings))
                return;

            var list = CalculateItems(this, settings.height);
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

        private static ColumnGenerationSettings CreateSettings(ColumnDraft draft)
        {
            var result = ColumnGenerationSettings.Create();

            result.length = draft.length;
            result.height = draft.height;

            result.isHorizontalMrror = draft.isHorizontalMrror;
            result.forwardScale = draft.forwardScale;

            result.parent = draft.parent;
            result.pivot = draft.pivot;

            result.up = draft.up;
            result.right = draft.right;
            result.forward = draft.forward;

            return result;
        }
        private static List<ColumnItem> CalculateItems(StandardColumn column, float height)
        {
            var totalMinHeight = GetTotalMinHeight(column);
            if (height < totalMinHeight)
            {
                var bottomItems = column.bottom.GetRequiredItems();
                var middleItems = column.middle.GetRequiredItems();
                var topItems = column.top.GetRequiredItems();
                return Utils.Combine(bottomItems, middleItems, topItems);
            }
            else
            {
                var bottomItems = column.bottom.GetValidItems();
                var topItems = column.top.GetValidItems();

                var edgesHeight = 0f;
                edgesHeight += Utils.CalculateHeightSum(bottomItems);
                edgesHeight += Utils.CalculateHeightSum(topItems);
                var middleHeight = height - edgesHeight;

                var items = column.middle.GetValidItems();
                var count = Utils.CalculateItemsCountByHeight(items, middleHeight);
                var middleItems = Utils.FillRepeated(items, count);

                return Utils.Combine(bottomItems, middleItems, topItems);
            }
        }

        private static void GenerateItems(List<ColumnItem> list, ColumnGenerationSettings settings)
        {
            var totalHeight = Utils.CalculateHeightSum(list);
            var heightscale = settings.height / totalHeight;

            var elementDraft = CreateElementDraft(settings);
            foreach (var item in list)
            {
                elementDraft.height = item.builder.height * heightscale;

                item.builder.Build(elementDraft);
                elementDraft.pivot += elementDraft.up * elementDraft.height;
            }
        }
        private static ElementDraft CreateElementDraft(ColumnGenerationSettings settings)
        {
            var result = ElementDraft.Create();

            result.length = settings.length;
            result.height = settings.height;
            
            result.isHorizontalMrror = settings.isHorizontalMrror;
            result.isVerticalMirror = false;
            result.forwardScale = settings.forwardScale;

            result.parent = settings.parent;
            result.pivot = settings.pivot;

            result.up = settings.up;
            result.right = settings.right;

            return result;
        }

        private static float GetTotalMinHeight(StandardColumn column)
        {
            var result = 0f;

            var items = column.bottom.GetRequiredItems();
            result += Utils.CalculateHeightSum(items);

            items = column.middle.GetRequiredItems();
            result += Utils.CalculateHeightSum(items);

            items = column.top.GetRequiredItems();
            result += Utils.CalculateHeightSum(items);

            return result;
        }
        private static float SelectMaxLength(StandardColumn column)
        {
            var items = column.bottom.GetValidItems();
            var bottomMax = Utils.SelectMaxLength(items);

            items = column.middle.GetValidItems();
            var middleMax = Utils.SelectMaxLength(items);

            items = column.top.GetValidItems();
            var topMax = Utils.SelectMaxLength(items);

            return Mathf.Max(bottomMax, middleMax, topMax);
        }
        private static float SelectMaxDepth(StandardColumn column)
        {
            var items = column.bottom.GetValidItems();
            var bottomMax = Utils.SelectMaxDepth(items);

            items = column.middle.GetValidItems();
            var middleMax = Utils.SelectMaxDepth(items);

            items = column.top.GetValidItems();
            var topMax = Utils.SelectMaxDepth(items);

            return Mathf.Max(bottomMax, middleMax, topMax);
        }

        private static float CalculateLengthScale(StandardColumn column, float length)
        {
            if (Utils.ApproximatelyZero(length, column.length))
                return 1f;

            return length / column.length;
        }
        private static float CalculateHeightScale(StandardColumn column, float height)
        {
            if (Utils.ApproximatelyZero(height, column.height))
                return 1f;

            var items = CalculateItems(column, height);
            var totalHeight = Utils.CalculateHeightSum(items);
            if (Utils.ApproximatelyZero(totalHeight))
                return 1f;

            return height / totalHeight;
        }

        /// ======================================================================

        private StandardColumn()
        {
            Reset();
        }

        /// ======================================================================
    }
}