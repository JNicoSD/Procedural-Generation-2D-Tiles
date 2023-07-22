using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleRandomizer
{
    public static class WeightedRandom
    {
        private static float totalSum = 0;
        // Randomizer that will return a random index of the object passed based on random weight
        // The list passed should contain the weight of the index
        // Example: 
        // index 0: 7 - will have 7/42 chances => ~16.67%
        // index 1: 10 - will have 10/42 chances => ~23.81%
        // index 2: 25 - will have 25/42 chances => ~59.52%
        
        public static int WeightedRandomIndex(List<float> weightList)
        {
            totalSum = 0;
            var sortedWeight = weightList.Select((item, index) => new {item, index}).OrderBy(o => o.item).ToDictionary(o => o.index, o => o.item);

            return CompareResult<int>(sortedWeight).Key;

            /*foreach(var weight in sortedWeight)
            {
                totalSum += weight.Value;
            }

            float compare = 0f;
            float resultNumber = UnityEngine.Random.Range(0, totalSum);

            foreach(var weight in sortedWeight)
            {
                compare += weight.Value;

                if(resultNumber < compare) return weight.Key;
            }

            

            // IF NONE OF THE ABOVE SATISFIES THE CONDITION, though it should not really reach here
            //return -1;*/
        }

        public static float WeightedRandomValue(List<float> weightList)
        {
            totalSum = 0;
            var sortedWeight = weightList.Select((item, index) => new {item, index}).OrderBy(o => o.item).ToDictionary(o => o.index, o => o.item);

            return CompareResult<int>(sortedWeight).Value;

            /* foreach(var weight in sortedWeight)
            {
                totalSum += weight.Value;
            }

            float compare = 0f;
            float resultNumber = UnityEngine.Random.Range(0, totalSum - 1);

            foreach(var weight in sortedWeight)
            {
                compare += weight.Value;

                if(resultNumber < compare) return weight.Value;
            }

            // IF NONE OF THE ABOVE SATISFIES THE CONDITION, though it  should not really reach here
            return -1; */
        }

        public static TKey WeightedRandomKey<TKey>(IDictionary<TKey, float> objectList) 
        {
            totalSum = 0;
            var sortedWeight = objectList.OrderBy(o => o.Value).ToDictionary(o => o.Key, o => o.Value);

            return CompareResult<TKey>(sortedWeight).Key;

            /* foreach(var weight in sortedWeight)
            {
                totalSum += weight.Value;
            }

            float compare = 0;
            float resultNumber = UnityEngine.Random.Range(0, totalSum - 1);

            foreach(var obj in sortedWeight)
            {
                compare += obj.Value;

                if(resultNumber < compare) return obj.Key;
            }

            // IF NONE OF THE ABOVE SATISFIES THE CONDITION, though it  should not really reach here
            return default(T); */
        }

        public static float WeightedRandomValue<TKey>(IDictionary<TKey, float> objectList)
        {
            totalSum = 0;
            var sortedWeight = objectList.OrderBy(o => o.Value).ToDictionary(o => o.Key, o => o.Value);

            return CompareResult<TKey>(sortedWeight).Value;

            /* foreach(var weight in sortedWeight)
            {
                totalSum += weight.Value;
            }

            float compare = 0;
            float resultNumber = UnityEngine.Random.Range(0, totalSum - 1);

            foreach(var weight in sortedWeight)
            {
                compare += weight.Value;

                if(resultNumber < compare) return weight.Value;
            }

            // IF NONE OF THE ABOVE SATISFIES THE CONDITION, though it  should not really reach here
            return -1; */
        }

        static KeyValuePair<TKey, float> CompareResult<TKey>(Dictionary<TKey, float> sortedWeight)
        {
            foreach(var weight in sortedWeight)
            {
                totalSum += weight.Value;
            }

            float compare = 0f;
            float resultNumber = UnityEngine.Random.Range(0, totalSum);

            foreach(var weight in sortedWeight)
            {
                compare += weight.Value;

                if(resultNumber < compare) return weight;
            }

            // IF NONE OF THE ONES ABOVE SATISFIES THE CONDITION
            KeyValuePair<TKey, float> tvp = new KeyValuePair<TKey, float>(typeof(TKey) == typeof(int) || typeof(TKey) == typeof(float) 
                                                                                                ? (TKey)(object)-1 
                                                                                                : default(TKey), -1f);
            return tvp;
        }
    }
}
