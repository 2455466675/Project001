using UnityEngine;

namespace GameFramework
{
    public static class MDebug
    {
        public static void Log(params object[] logs)
        {
            if (logs == null || logs.Length <= 0)
            {
                return;
            }
            Debug.Log($"<color=#1D802D>[frameCount:{Time.frameCount}]---Message---</color>{string.Join(" ", logs)}");
        }

        public static void Error(params object[] logs)
        {
            if (logs == null || logs.Length <= 0)
            {
                return;
            }
            Debug.LogError(string.Join(" ", logs));
        }

        public static void Warn(params object[] logs)
        {
            if (logs == null || logs.Length <= 0)
            {
                return;
            }
            Debug.LogWarning(string.Join(" ", logs));
        }
    }
}
