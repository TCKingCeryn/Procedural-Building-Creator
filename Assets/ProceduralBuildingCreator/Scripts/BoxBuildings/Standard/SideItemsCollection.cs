using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Boxes.Standard
{
    [Serializable]
    public class SideItemsCollection : IValidable
    {
        /// ======================================================================

        [SerializeField] private List<SideItem> _items;

        /// ======================================================================

        private List<SideItem> items
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
            _items = new List<SideItem>();
        }
        
        public List<SideItem> GetValidItems()
        {
            return Utils.PickValid(items);
        }
        public SideItem GetValidItem(int index)
        {
            var validItems = GetValidItems();
            return Utils.GetRepeatedItem(validItems, index);
        }

        /// ======================================================================

        private SideItemsCollection()
        {
            Reset();
        }
        public static SideItemsCollection Create()
        {
            return new SideItemsCollection();
        }

        /// ======================================================================
    }
}