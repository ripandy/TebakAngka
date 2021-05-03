using Kassets.Collection;
using UnityEngine;

namespace Feature.GridManagement
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Shuffle a Collection
        /// </summary>
        /// <param name="collection"></param>
        /// <typeparam name="T"></typeparam>
        public static void Shuffle<T>(this Collection<T> collection)
        {
            for (var n = collection.Count - 1; n > 0; --n)
            {
                var k = Random.Range(0, n + 1);
                collection.Swap(n, k);
            }
        }

        /// <summary>
        /// Swap Collection's value on index1 with value on index2
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <typeparam name="T"></typeparam>
        public static void Swap<T>(this Collection<T> collection, int index1, int index2)
        {
            var temp = collection[index1];
            collection[index1] = collection[index2];
            collection[index2] = temp;
        }

        /// <summary>
        /// Get a value of Collection that is assumed to have a set of rows with column count of gridWidth. 
        /// </summary>
        /// <param name="collection">A Collection that is designed with rows and columns</param>
        /// <param name="x">column number</param>
        /// <param name="y">row number</param>
        /// <param name="gridWidth">number of column the Collection has</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetValueFromGrid<T>(this Collection<T> collection, int x, int y, int gridWidth)
        {
            return collection[y * gridWidth + x];
        }
    }
}