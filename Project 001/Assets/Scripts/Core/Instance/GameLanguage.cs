using Game.Cfg;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Game.Core
{
    public class LanguageItem 
    {
        public int id;
        public string text;
        public int colorId;
        public Color color;       
    }

    /// <summary>
    /// 
    /// </summary>
    public class GameLanguage : MonoBehaviour, ICore
    {
        public IEnumerator Init()
        {
            yield return null;
        }

        public string GetTextById(int id)
        {
            if (id == 0)
            {
                return string.Empty;
            }

            LanguageCfg cfg = GameCore.GameCfgData.FindById<LanguageCfg>(id);
            if (cfg == null)
            {
                return id.ToString();
            }
            return cfg.text;
        }

        public LanguageItem GetLanguageItem(int id)
        {            
            LanguageCfg cfg = GameCore.GameCfgData.FindById<LanguageCfg>(id);            
            if(cfg == null)
            {
                return null;
            }

            ColorCfg colorCfg = GameCore.GameCfgData.FindById<ColorCfg>(cfg.color);
            if (colorCfg == null)
            {
                MLog.Error($"没有此文本颜色:{cfg.color}");
                return null; 
            }

            LanguageItem item = new()
            {
                id = id,
                text = cfg.text,
                colorId = cfg.color
            };

            if (ColorUtility.TryParseHtmlString($"#{colorCfg.color}", out Color color))
            {
                item.color = color;
            }
            else
            {
                item.color = Color.black;
   
            }
            return item;
        }
    }
}