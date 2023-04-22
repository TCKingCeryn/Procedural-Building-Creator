using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework
{
	public static class Utils 
	{
        /// ======================================================================

        public static bool IsValid(IValidable obj)
        {
            if (obj == null)
                return false;

            return obj.IsValid();
        }
        public static bool AnyValid(params IValidable[] array)
        {
            foreach (var item in array)
            {
                if (IsValid(item))
                    return true;
            }

            return false;
        }
        public static bool EachValid(params IValidable[] array)
        {
            foreach (var item in array)
            {
                if (!IsValid(item))
                    return false;
            }

            return true;
        }

        public static bool NotNullAndHasElements(IList list)
        {
            if (list == null)
                return false;

            if (list.Count == 0)
                return false;

            return true;
        }
        public static bool HasAnyValidItems<T>(List<T> list) where T : IValidable 
        {
            if (!NotNullAndHasElements(list))
                return false;

            foreach (var item in list)
            {
                if (IsValid(item))
                    return true;
            }

            return false;
        }

        public static bool ApproximatelyZero(float value)
        {
            return Mathf.Approximately(value, 0f);
        }
        public static bool ApproximatelyZero(params float[] array)
        {
            foreach (var item in array)
            {
                if (ApproximatelyZero(item))
                    return true;
            }

            return false;
        }

        public static bool Negative(float value)
        {
            return value < 0f;
        }
        public static bool Negative(params float[] array)
        {
            foreach (var item in array)
            {
                if (item < 0f)
                    return true;
            }

            return false;
        }

        public static bool NegativeOrZero(float value)
        {
            if (value < 0f)
                return true;

            if (ApproximatelyZero(value))
                return true;

            return false;
        }
        public static bool NegativeOrZero(params float[] array)
        {
            foreach (var item in array)
            {
                if (NegativeOrZero(item))
                    return true;
            }

            return false;
        }

        public static float GetValidLength<T>(T obj) where T : IValidable, ILength
        {
            if (!IsValid(obj))
                return 0f;

            return obj.length;
        }
        public static float GetValidHeight<T>(T obj) where T : IValidable, IHeight
        {
            if (!IsValid(obj))
                return 0f;

            return obj.height;
        }
        public static float GetValidDepth<T>(T obj) where T : IValidable, IDepth
        {
            if (!IsValid(obj))
                return 0f;

            return obj.depth;
        }

        /// ======================================================================

        public static float SelectMaxHeight<T>(List<T> list) where T : IHeight
        {
            var result = 0f;

            if (!NotNullAndHasElements(list))
                return result;

            foreach (var item in list)
            {
                if (item.height > result)
                    result = item.height;
            }

            return result;
        }
        public static float SelectMaxLength<T>(List<T> list) where T : ILength
        {
            var result = 0f;

            if (!NotNullAndHasElements(list))
                return result;

            foreach (var item in list)
            {
                if (item.length > result)
                    result = item.length;
            }

            return result;
        }
        public static float SelectMaxDepth<T>(List<T> list) where T : IDepth
        {
            var result = 0f;

            if (!NotNullAndHasElements(list))
                return result;

            foreach (var item in list)
            {
                if (item.depth > result)
                    result = item.depth;
            }

            return result;
        }

        public static float CalculateHeightSum<T>(List<T> list) where T : IHeight
        {
            var result = 0f;

            if (!NotNullAndHasElements(list))
                return result;

            foreach (var item in list)
            {
                result += item.height;
            }

            return result;
        }
        public static float CalculateLengthSum<T>(List<T> list) where T : ILength
        {
            var result = 0f;

            if (!NotNullAndHasElements(list))
                return result;

            foreach (var item in list)
            {
                result += item.length;
            }

            return result;
        }

        public static int CalculateItemsCountByHeight<T>(List<T> list, float height) where T : IHeight
        {
            if (!NotNullAndHasElements(list))
                return 0;

            var size = 0f;
            var count = 0;

            var iterations = 100;
            var lerp = 0.5f;

            while (size < height && iterations > 0)
            {
                foreach (var item in list)
                {
                    if (size + item.height * lerp < height)
                    {
                        size += item.height;
                        count++;
                    }

                    iterations--;
                }
            }

            return count;
        }
        public static int CalculateItemsCountByLength<T>(List<T> list, float length) where T : ILength
        {
            if (!NotNullAndHasElements(list))
                return 0;

            var size = 0f;
            var count = 0;

            var iterations = 100;
            var lerp = 0.5f;

            while (size < length && iterations > 0)
            {
                foreach (var item in list)
                {
                    if (size + item.length * lerp < length)
                    {
                        size += item.length;
                        count++;
                    }

                    iterations--;
                }
            }

            return count;
        }

        public static float GetAverage(params float[] array)
        {
            if (array.Length == 0)
                return 0f;

            var sum = 0f;

            for (int i = 0; i < array.Length; i++)
            {
                sum += array[i];
            }

            return sum / array.Length;
        }

        /// ======================================================================

        public static List<T> PickValid<T>(List<T> list) where T : IValidable
        {
            var result = new List<T>();
            foreach (var item in list)
            {
                if (IsValid(item))
                    result.Add(item);
            }
            return result;
        }
        public static List<T> PickRequired<T>(List<T> list) where T : IRequired
        {
            var result = new List<T>();
            foreach (var item in list)
            {
                if (item.isRequired)
                    result.Add(item);
            }
            return result;
        }

        public static List<T> Combine<T>(params IEnumerable<T>[] array)
        {
            var result = new List<T>();
            foreach (var item in array)
            {
                result.AddRange(item);
            }
            return result;
        }

        public static T GetRepeatedItem<T>(List<T> list, int index)
        {
            if (!NotNullAndHasElements(list))
                return default(T);

            var i = index % list.Count;
            return list[i];
        }
        public static List<T> FillRepeated<T>(List<T> list, int count)
        {
            var result = new List<T>();
            for (int i = 0; i < count; i++)
            {
                var index = i % list.Count;
                result.Add(list[index]);
            }
            return result;
        }
       
        /// ======================================================================

        public static void RunDecorators(Transform transform)
        {
            if (transform == null)
                return;

            var array = transform.GetComponentsInChildren<IDecorator>();
            foreach (var decorator in array)
            {
                decorator.Decorate();
            }
        }

        public static T GetWeightRandom<T>(List<T> list) where T : IRandomizable<T>
        {
            if (!NotNullAndHasElements(list))
                return default(T);

            var sum = 0;
            foreach (var item in list)
            {
                sum += item.weight;
            }

            var weight = sum;
            var random = Random.Range(0, sum);
            foreach (var item in list)
            {
                weight -= item.weight;
                if (random >= weight)
                    return item;
            }

            return list[list.Count - 1];
        }
               
        /// ======================================================================

        public static GameObject Instantiate(GameObject prefab)
        {
            if (prefab == null)
                return null;

#if UNITY_EDITOR
            return (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab);
#else
            return Object.Instantiate(prefab);
#endif
        }

        public static void DeleteChilds(Transform transform)
        {
            if (transform == null)
                return;

            var childs = GetChilds(transform);
            for (int i = 0; i < childs.Length; i++)
            {
                childs[i].gameObject.SetActive(false);

#if UNITY_EDITOR
                Object.DestroyImmediate(childs[i].gameObject);
#else
                Object.Destroy(childs[i].gameObject);
#endif
            }
        }
        public static Transform[] GetChilds(Transform transform)
        {
            if (transform == null)
                return new Transform[0];

            var count = transform.childCount;
            var result = new Transform[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = transform.GetChild(i);
            }
            return result;
        }

        /// ======================================================================
    }
}