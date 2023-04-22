using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Elements.Standard
{
    // DONE
    [Serializable]
	public class ElementItemsCollection : IValidable
	{
        /// ======================================================================

        [SerializeField] private List<ElementItem> _items;

        /// ======================================================================

        private List<ElementItem> items
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
            _items = new List<ElementItem>();
        }

        public List<ElementItem> GetValidItems()
        {
            return Utils.PickValid(items);
        }
        public ElementItem GetRandomValidItem()
        {
            var validItems = GetValidItems();
            return Utils.GetWeightRandom(validItems);
        }

        /// ======================================================================

        private ElementItemsCollection()
        {
            Reset();
        }
        public static ElementItemsCollection Create()
        {
            return new ElementItemsCollection();
        }

        /// ======================================================================
    }
}