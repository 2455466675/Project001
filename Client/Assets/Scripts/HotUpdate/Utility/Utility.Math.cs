using UnityEngine;

namespace GameFramework.Utility
{
    public static class GameMath
    {
        public static int Max(int v1, int v2)
        {
            return Mathf.Max(v1, v2);
        }

        public static int Min(int v1, int v2)
        {
            return Mathf.Min(v1, v2);
        }

        public static float Min(float v1, float v2)
        {
            return Mathf.Min(v1, v2);
        }

        public static int Floor(float v)
        {
            return Mathf.FloorToInt(v);
        }

        public static int Ceil(float v)
        {
            return Mathf.CeilToInt(v);
        }

        public static float Abs(float v)
        {
            return Mathf.Abs(v);
        }

        public static int Abs(int v)
        {
            return Mathf.Abs(v);
        }

        /// <summary>
        /// [min, max)
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}
