using UnityEngine;

namespace GameFramework
{
    public static class MDebug
    {
        // 使用不同颜色区分日志等级，便于在控制台快速定位不同重要程度的信息
        public static void Log(object message, int level = 1)
        {
            if (message == null)
            {
                return;
            }

            string color;
            switch (level)
            {
                case 2:
                    color = "#1E90FF"; // 蓝色
                    break;
                case 3:
                    color = "#FF8C00"; // 橙色
                    break;
                case 4:
                    color = "#FF0000"; // 红色
                    break;
                default:
                    color = "#1D802D"; // 绿色
                    break;
            }

            Debug.Log($"<color={color}>[frameCount:{Time.frameCount}]---Message---</color>{message}");
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
