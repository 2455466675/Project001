using System.Collections.Generic;
using UnityEngine;

namespace Game.Cfg
{
    public class LanguageItem 
    {
        public string TextValue { get; private set; }
        public Color ColorValue { get; private set; }

        public LanguageItem(string text, Color color) 
        { 
            TextValue = text;
            ColorValue = color;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GameLanguage 
    {
        public Color DefaultTextColor => Color.black;

        private readonly Dictionary<int, LanguageItem> languageItems;
        private readonly Dictionary<string, Color> colorMap;

        public GameLanguage()
        {
            languageItems = new Dictionary<int, LanguageItem>();
            colorMap = new Dictionary<string, Color>();
        }

        public string GetTextById(int id)
        {
            LanguageItem item = GetLanguageItem(id);
            if (item == null)
            {
                return id.ToString();
            }
            
            return item.TextValue;
        }

        public LanguageItem GetLanguageItem(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            if (languageItems.ContainsKey(id))
            {
                return languageItems[id];
            }

            LanguageCfg cfg = GameCore.Cfg.Find<LanguageCfg>(id);            
            if(cfg == null)
            {
                //MLog.Error($"没有此语言配置:{id}");
                return null;
            }

            LanguageItem item = new(cfg.Text, GetColorById(cfg.Color));
            languageItems.Add(id, item);
 
            return item;
        }

        public Color GetColorById(int colorId)
        {
            ColorCfg cfg = GameCore.Cfg.Find<ColorCfg>(colorId);
            if (cfg == null)
            {
                MLog.Error($"没有此颜色配置:{colorId}");
                return DefaultTextColor;
            }
            
            return GetColorByHtmlStr(cfg.Color);
        }

        public Color GetColorByHtmlStr(string htmlStr)
        {
            if (!htmlStr.StartsWith('#'))
            {
                htmlStr = $"#{htmlStr}";
            }

            if (colorMap.ContainsKey(htmlStr))
            {
                return colorMap[htmlStr];
            }
                        
            if (ColorUtility.TryParseHtmlString(htmlStr, out Color color))
            {
                colorMap.Add(htmlStr, color);
                return color;
            }
            else
            {
                MLog.Error($"颜色转化失败:{htmlStr}");
                return DefaultTextColor;
            }
        }
    }
}