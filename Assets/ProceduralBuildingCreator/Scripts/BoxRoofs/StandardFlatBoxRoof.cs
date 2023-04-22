using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;
using ModularBuildingsFramework.Roofs.StandardFlatBox;

namespace ModularBuildingsFramework.Roofs
{
	public class StandardFlatBoxRoof : ScriptableObject, IBoxBuilder
	{
        /// ======================================================================

        [SerializeField] protected RoofParameters _parameters;
        [SerializeField] protected GameObject _prefab;

        /// ======================================================================

        public float length
        {
            get { return parameters.length; }
        }
        public float height
        {
            get { return 0f; }
        }
        public float depth
        {
            get { return parameters.depth; }
        }

        private RoofParameters parameters
        {
            get { return _parameters; }
        }
        private GameObject prefab
        {
            get { return _prefab; }
        }

        /// ======================================================================

        public bool IsValid()
        {
            if (prefab == null)
                return false;

            return Utils.IsValid(parameters);
        }
        public void Reset()
        {
            _parameters = RoofParameters.Create();
            _prefab = default(GameObject);
        }

        public void Build(BoxDraft draft)
        {
            if (!Utils.IsValid(this))
                return;

            if (!Utils.IsValid(draft))
                return;

            var settings = CreateSettings(parameters, draft);
            if (!Utils.IsValid(settings))
                return;

            var action = GetGenerationAction(parameters.mode);
            action(prefab, settings);
        }

        public float CalculateLengthScale(float length)
        {
            return 1f;
        }
        public float CalculateHeightScale(float height)
        {
            return 1f; 
        }

        /// ======================================================================

        private static RoofGenerationSettings CreateSettings(RoofParameters parameters, BoxDraft draft)
        {
            var result = RoofGenerationSettings.Create();

            result.itemLength = parameters.length;
            result.itemDepth = parameters.depth;

            result.targetLength = draft.length;
            result.targetDepth = draft.depth;

            result.parent = draft.parent;
            result.pivot = draft.pivot;

            result.up = draft.up;
            result.right = draft.right;
            result.forward = draft.forward;

            return result;
        }
        private Action<GameObject, RoofGenerationSettings> GetGenerationAction(RoofMode mode)
        {
            switch (mode)
            {
                case RoofMode.Scale: return GenerateItemScale;
                case RoofMode.Tile: return GenerateItemTiled;
                default:
                    throw new Exception(mode.ToString());
            }
        }

        private static void GenerateItemScale(GameObject prefab, RoofGenerationSettings settings)
        {
            var scaleX = settings.targetLength / settings.itemLength;
            var scaleZ = settings.targetDepth / settings.itemDepth;

            var gameObject = Utils.Instantiate(prefab);
            if (gameObject == null)
                return;

            var transform = gameObject.transform;
            transform.parent = settings.parent;

            transform.position = settings.pivot;
            transform.eulerAngles = Quaternion.LookRotation(settings.forward, settings.up).eulerAngles;
            transform.localScale = new Vector3(scaleX, 1, scaleZ);
        }
        private static void GenerateItemTiled(GameObject prefab, RoofGenerationSettings settings)
        {
            var countX = CalculateTiledCount(settings.itemLength, settings.targetLength);
            if (countX < 1)
                countX = 1;

            var countZ = CalculateTiledCount(settings.itemDepth, settings.targetDepth);
            if (countZ < 1)
                countZ = 1;

            var totalLength = countX * settings.itemLength;
            var totalDepth = countZ * settings.itemDepth;

            var scaleX = settings.targetLength / totalLength;
            var scaleZ = settings.targetDepth / totalDepth;

            var deltaX = settings.targetLength / countX;
            var deltaZ = settings.targetDepth / countZ;

            for (int z = 0; z < countZ; z++)
            {
                for (int x = 0; x < countX; x++)
                {
                    var gameObject = Utils.Instantiate(prefab);
                    if (gameObject == null)
                        continue;

                    var transform = gameObject.transform;
                    transform.parent = settings.parent;

                    transform.position = settings.pivot
                        + settings.right * deltaX * x
                        + settings.forward * deltaZ * z
                        ;

                    transform.eulerAngles =
                        Quaternion.LookRotation(settings.forward, settings.up).eulerAngles;

                    transform.localScale = new Vector3(
                        scaleX,
                        1,
                        scaleZ
                        );
                }
            }
        }
        private static int CalculateTiledCount(float currentSize, float targetSize)
        {
            if (Utils.ApproximatelyZero(currentSize, targetSize))
                return 0;

            return Mathf.RoundToInt(targetSize / currentSize);
        }

        /// ======================================================================

        private StandardFlatBoxRoof()
        {
            Reset();
        }

        /// ======================================================================
    }
}