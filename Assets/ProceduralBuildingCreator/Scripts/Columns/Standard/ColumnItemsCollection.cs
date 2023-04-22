using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Columns.Standard
{
    // DONE
    [Serializable]
	public class ColumnItemsCollection : IValidable
	{
        /// ======================================================================

        [SerializeField] private List<ColumnItem> _items;

        /// ======================================================================

        private List<ColumnItem> items
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
            _items = new List<ColumnItem>();
        }

        public List<ColumnItem> GetValidItems()
        {
            return Utils.PickValid(items);
        }
        public List<ColumnItem> GetRequiredItems()
        {
            var validItems = GetValidItems();
            return Utils.PickRequired(validItems);
        }

        /// ======================================================================

        private ColumnItemsCollection()
        {
            Reset();
        }
        public static ColumnItemsCollection Create()
        {
            return new ColumnItemsCollection();
        }

        /// ======================================================================
    }
}