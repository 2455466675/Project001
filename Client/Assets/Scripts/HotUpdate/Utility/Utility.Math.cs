using UnityEngine;

namespace GameFramework.Utility
{
    public static partial class Util
    {
        public static class Math
        {
            public static int Max(int v1, int v2) 
            {
                return Mathf.Max(v1, v2);
            }

            public static int Min(int v1, int v2)
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
        }
    }
}