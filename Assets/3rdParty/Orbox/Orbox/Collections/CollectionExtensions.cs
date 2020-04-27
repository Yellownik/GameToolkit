using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Orbox.Collections
{
    public static class CollectionExtensions
    {
        //---------   Select collection item by minimum or maximum item.field value   ---------
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return ExtremumBy<TSource, TKey>(source, selector, value => value < 0);
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return ExtremumBy<TSource, TKey>(source, selector, value => value > 0);
        }



        //---------   Find item that has nearest   in collection item.field value  ---------
        public static TSource NearestTo<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector, float target) //where TKey: float
        {
            Comparison<float> compare = (a, b) =>
            {
                var distanceA = Math.Abs(a - target);
                var distanceB = Math.Abs(b - target);

                return distanceA.CompareTo(distanceB);
            };

            return ExtremumBy<TSource, float>(source, selector, value => value < 0, compare);
        }

        public static TSource NearestTo<TSource>(this IEnumerable<TSource> source, Func<TSource, Vector3> selector, Vector3 target) //where TKey: float
        {
            Comparison<Vector3> compare = (a, b) =>
            {
                var distanceA = (a - target).sqrMagnitude;
                var distanceB = (b - target).sqrMagnitude;

                return distanceA.CompareTo(distanceB);
            };

            return ExtremumBy<TSource, Vector3>(source, selector, value => value < 0, compare);
        }


        //---------   FindAll and convert to component list   ---------        
        public static  List<TResult> FindAllAndConvert<TResult, TSource>(this List<TSource> items, Predicate<TSource> match)
            where TResult : MonoBehaviour
            where TSource : MonoBehaviour
        {
            var result = new List<TResult>();
            var list = items.FindAll(match);

            foreach (var item in list)
            {
                var component = item.GetComponent<TResult>();
                result.Add(component);
            }

            return result;
        }

        public static List<T> ConvertToList<T>(this Transform transform) where T : MonoBehaviour
        {
            var list = new List<T>();

            foreach (Transform item in transform)
            {
                var component = item.GetComponent<T>();
                list.Add(component);
            }

            return list;
        }



        //---------   Unity ios safe analog linq ToList()    ---------
        public static List<T> ToList<T>(this IEnumerable<T> items)
        {
            var list = new List<T>();

            foreach(var item in items)
            {
                list.Add(item);
            }

            return list;
        }

        public static List<T> ToList<T>(this IEnumerable items)
        {
            var list = new List<T>();

            foreach (var item in items)
            {
                list.Add((T)item);
            }

            return list;
        }




        //---------   private methods block   ---------
        private static TSource ExtremumBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, Func<int, bool> extremum)
        {
            var comparer = Comparer<TKey>.Default;
            Comparison<TKey> compare = comparer.Compare;

            return ExtremumBy<TSource, TKey>(source, selector, extremum, compare);
        }

        private static TSource ExtremumBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, Func<int, bool> extremum, Comparison<TKey> compare)
        {
            using (var iterator = source.GetEnumerator())
            {
                if (iterator.MoveNext() == false) throw new InvalidOperationException("Sequence was empty");

                TSource item = iterator.Current;
                TKey key = selector(item);

                while (iterator.MoveNext())
                {
                    TSource candidate = iterator.Current;
                    TKey candidateKey = selector(candidate);

                    int result = compare(candidateKey, key);

                    if (extremum(result) == true)
                    {
                        item = candidate;
                        key = candidateKey;
                    }
                }

                return item;
            }
        }        

    }
}