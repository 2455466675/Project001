using Game.Cfg;
using System.Collections;
using UnityEngine;

namespace Game.Core
{
    public struct LanguageItem 
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
            return cfg.Text;
        }

        public LanguageItem? GetLanguageItem(int id)
        {            
            LanguageCfg cfg = GameCore.GameCfgData.FindById<LanguageCfg>(id);            
            if(cfg == null)
            {
                return null;
            }

            ColorCfg colorCfg = GameCore.GameCfgData.FindById<ColorCfg>(cfg.Color);
            if (colorCfg == null)
            {
                MLog.Error($"没有此文本颜色:{cfg.Color}");
                return null; 
            }

            LanguageItem item = new()
            {
                id = id,
                text = cfg.Text,
                colorId = cfg.Color
            };

            if (ColorUtility.TryParseHtmlString($"#{colorCfg.Color}", out Color color))
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