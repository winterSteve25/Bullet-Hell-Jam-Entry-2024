using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class RandomUtils
    {
        public static int RangeExcludeMany(int min, int maxExc, params int[] exclude)
        {
            List<int> availableValues = new List<int>();
            for (int i = min; i < maxExc; i++)
            {
                if (System.Array.IndexOf(exclude, i) == -1)
                {
                    availableValues.Add(i);
                }
            }

            return availableValues[Random.Range(0, availableValues.Count)];
        }
    }
}
