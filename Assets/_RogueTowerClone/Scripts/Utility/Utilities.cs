using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utilities
{
    public static T GetRandomElement<T>(this IEnumerable<T> collection)
    {
        int count = collection.Count();
        System.Random _random = new System.Random();

        if (count == 0)
        {
            return default;
        }

        return collection.ElementAt(_random.Next(0, count));
    }
}