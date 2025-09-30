using UnityEngine;

namespace GameFramework
{
    public static partial class Utility
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
        }
    }
}