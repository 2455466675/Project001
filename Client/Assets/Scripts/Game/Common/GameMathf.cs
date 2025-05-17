using Cysharp.Threading.Tasks;
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

        public static float Lerp(float startValue, float endValue, float t) 
        {
            return UnityEngine.Mathf.Lerp(startValue, endValue, t);
        }

        public static float Pow(float f, float p) 
        {
            return UnityEngine.Mathf.Pow(f, p);
        }

        public static async UniTask Lerp(float startValue, float endValue, float duration, Action<float> onValueChanged, EasingFunction easingFunction = null)
        {
            easingFunction ??= Easing.Linear;

            float elapsedTime = 0f;
            onValueChanged?.Invoke(startValue);

            while (elapsedTime < duration)
            {        
                float t = elapsedTime / duration;
                t = easingFunction(t);
                float value = Lerp(startValue, endValue, t);

                onValueChanged?.Invoke(value);

                await UniTask.Yield(PlayerLoopTiming.Update);
                elapsedTime += UnityEngine.Time.deltaTime;
            }

            onValueChanged?.Invoke(endValue);
        }

        // 缓动函数定义
        public static class Easing
        {
            public static float Linear(float t) => t;
            // 加速入场
            public static float EaseInQuad(float t) => t * t;
            // 减速退场 
            public static float EaseOutQuad(float t) => 1 - (1 - t) * (1 - t);
            // 组合效果
            public static float EaseInOutQuad(float t) => t < 0.5 ? 2 * t * t : 1 - Pow(-2 * t + 2, 2) / 2;
            //三次缓动（Cubic）
            public static float EaseInCubic(float t) => t * t * t;
            public static float EaseOutCubic(float t) => 1 - Pow(1 - t, 3);
        }

        public delegate float EasingFunction(float t);
    }
}