using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Lines.Standard
{
    [Serializable]
	public class LineItemsCollection : IValidable
	{
        /// ======================================================================

        [SerializeField] private List<LineItem> _items;

        /// ======================================================================

        private List<LineItem> items
        {
            get { return _items; }
        }

        /// ======================================================================

        public bool IsValid()
        {
            return Utils.HasAnyValidItems(items);
        }
        public void Reset()
        {
            _items = new List<LineItem>();
        }

        public List<LineItem> GetValidItems()
        {
            return Utils.PickValid(items);
        }
        public List<LineItem> GetRequiredItems()
        {
            var validItems = GetValidItems();
            return Utils.PickRequired(validItems);
        }

        public float GetAvergeScaleByHeight(float height)
        {
            if (!IsValid())
                return 1f;

            var items = GetValidItems();
            var array = new float[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                array[i] = items[i].builder.CalculateHeightScale(height);
            }
            return Utils.GetAverage(array);
        }

        /// ======================================================================

        private LineItemsCollection()
        {
            Reset();
        }
        public static LineItemsCollection Create()
        {
            return new LineItemsCollection();
        }

        /// ======================================================================
    }
}