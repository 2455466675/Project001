using System;

namespace Game
{
    public static class GameMathf
    {
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
    }
}