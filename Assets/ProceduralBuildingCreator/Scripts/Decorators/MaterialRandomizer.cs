using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using ModularBuildingsFramework.Decorators.Materials;

namespace ModularBuildingsFramework.Decorators
{
	public class MaterialRandomizer : MonoBehaviour, IValidable, IDecorator
	{
        /// ======================================================================

        [SerializeField] private List<Renderer> _renderers;
        [SerializeField] private List<MaterialRandomizerItem> _materials;

        /// ======================================================================

        private List<Renderer> renderers
        {
            get { return _renderers; }
        }
        private List<MaterialRandomizerItem> materials
        {
            get { return _materials; }
        }

        /// ======================================================================

        public bool IsValid()
        {
            if (!Utils.NotNullAndHasElements(renderers))
                return false;

            if (!Utils.HasAnyValidItems(materials))
                return false;

            return Utils.HasAnyValidItems(materials);
        }
        public void Reset()
        {
            _renderers = new List<Renderer>();
            _materials = new List<MaterialRandomizerItem>();
        }

        public void Decorate()
        {
            if (!IsValid())
                return;
            
            var validItems = Utils.PickValid(materials);
            foreach (var renderer in renderers)
            {
                if (renderer == null)
                    continue;

                var item = Utils.GetWeightRandom(validItems);
                renderer.sharedMaterial = item.material;
            }
        }

        /// ======================================================================

        private MaterialRandomizer()
        {
            
        }

        /// ======================================================================
    }
}