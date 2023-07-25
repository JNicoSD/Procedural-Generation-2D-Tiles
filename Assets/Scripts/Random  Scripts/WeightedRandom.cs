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
        /// Randomizer that will return a random index of the object passed based on random weight
        /// The list passed should contain the weight of the index
        /// Example: 
        /// index 0: 7 - will have 7/42 chances => ~16.67%
        /// index 1: 10 - will have 10/42 chances => ~23.81%
        /// index 2: 25 - will have 25/42 chances => ~59.52%
        
        public static int WeightedRandomIndex(List<float> weightList)
        {
            totalSum = 0;
            var sortedWeight = weightList.Select((item, index) => new {item, index}).OrderBy(o => o.item).ToDictionary(o => o.index, o => o.item);

            return CompareResult<int>(sortedWeight).Key;
        }

        public static float WeightedRandomValue(List<float> weightList)
        {
            totalSum = 0;
            var sortedWeight = weightList.Select((item, index) => new {item, index}).OrderBy(o => o.item).ToDictionary(o => o.index, o => o.item);

            return CompareResult<int>(sortedWeight).Value;
        }

        public static TKey WeightedRandomKey<TKey>(IDictionary<TKey, float> objectList) 
        {
            totalSum = 0;
            var sortedWeight = objectList.OrderBy(o => o.Value).ToDictionary(o => o.Key, o => o.Value);

            return CompareResult<TKey>(sortedWeight).Key;

        }

        public static float WeightedRandomValue<TKey>(IDictionary<TKey, float> objectList)
        {
            totalSum = 0;
            var sortedWeight = objectList.OrderBy(o => o.Value).ToDictionary(o => o.Key, o => o.Value);

            return CompareResult<TKey>(sortedWeight).Value;
        }

        /// <summary>
        /// CompareResult will...compare the float compare to the weight Value and see if it falls on a certain range
        /// </summary>
        /// <typeparam name="TKey"> the type of the key passed </typeparam>
        /// <param name="sortedWeight"> A dictionary of the collections passed that was sorted in order based on the weight (lowest to highest) </param>
        /// <returns></returns>
        static KeyValuePair<TKey, float> CompareResult<TKey>(Dictionary<TKey, float> sortedWeight)
        {   
            /// How this works:
            /// 1. Get the totalSum of all the weight
            /// 2. Create a float 'compare' = 0f as a starting value
            /// 3. Randomize a number from 0 up to totalSum. The result will be in the range 0 - (totalSum-1)
            /// 4. using 'compare', add the weight of a TKey and see if the value of compare is lower than the result number
            /// This is why the sorting the weight from lowest to highest is important
            /// Also, since we start at 0, any TKey with weight 0 will never satisfy the if statement since (0 < 0) will always be false

            /// Example scenario:
            /// index 0: 0 
            /// index 1: 7 
            /// index 2: 10
            /// index 3: 25 
            /// 1. 0 + 7 + 10 + 25 = 42 which will be assigned to totalSum
            /// 2. compare = 0f
            /// 3. Random.Range(0, 42) which will result a number from 0 - 41. For now, let's assume the resultNumber = 7
            /// 4.0 compare = 0 => compare += 0 => 0 and 7 < 0 is false, which means index 0 is not the selected index
            /// 4.1 compare = 0 => compare += 7 => 7 and 7 < 7 is false, which means index 1 is not the selected index
            /// 4.2 compare = 7 => compare += 10 => 17 and 7 < 17 is true, which means index 2 is the selected index
            /// There is no nedd to test index 3 since we already found the selected index

            /// Another way to visualize this is:
            /// index 0 has no range since weight is 0      
            /// index 1 range is 0 - 6                      
            /// index 2 range is 7 - 16                     
            /// index 3 range is 17 - 41                    
            /// Since resultNumber is 7, it falls under index 2


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

            /// IF NONE OF THE ONES ABOVE SATISFIES THE CONDITION, return a default value for TKey, and -1 for the value
            /// Though, the code should never reach this unless all TKey has weight 0
            KeyValuePair<TKey, float> tvp = new KeyValuePair<TKey, float>(typeof(TKey) == typeof(int) || typeof(TKey) == typeof(float) 
                                                                                                ? (TKey)(object)-1 
                                                                                                : default(TKey), -1f);
            return tvp;
        }
    }
}
