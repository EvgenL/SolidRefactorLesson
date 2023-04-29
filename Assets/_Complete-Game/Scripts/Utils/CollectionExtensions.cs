using System.Collections.Generic;
using UnityEngine;

namespace Completed
{
    public static class CollectionExtensions
    {
        public static T GetRandomElement<T>(this IList<T> collection)
        {
            return collection[Random.Range(0, collection.Count)];
        }
    }
}