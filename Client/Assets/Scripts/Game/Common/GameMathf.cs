using System;

namespace Game
{
    public static class GameMathf
    {
        public static double PI => Math.PI;

        public static int Max(int a, int b)
        {
            return (a > b) ? a : b;
        }

        public static float Max(float a, float b)
        {
            return (a > b) ? a : b;
        }

        public static int Min(int a, int b)
        {
            return (a < b) ? a : b;
        }

        public static float Min(float a, float b)
        {
            return (a < b) ? a : b;
        }

        public static int Abs(int v) 
        {
            return Math.Abs(v);
        }

        public static float Abs(float f)
        {
            return Math.Abs(f);
        }

        private static Random random = new Random();

        /// <summary>
        /// [min, max)
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Random(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}