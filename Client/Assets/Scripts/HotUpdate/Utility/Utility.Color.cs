using UnityEngine;

namespace GameFramework.Utility
{
    public static partial class Util
    {
        public static class Color
        {
            public static UnityEngine.Color DefaultColor => UnityEngine.Color.black;

            public static UnityEngine.Color GetColorByHtmlStr(string htmlStr)
            {
                if (!htmlStr.StartsWith('#'))
                {
                    htmlStr = $"#{htmlStr}";
                }
                if (ColorUtility.TryParseHtmlString(htmlStr, out UnityEngine.Color color))
                {
                    return color;
                }
                else
                {
                    return DefaultColor;
                }
            }
        }
    }
}