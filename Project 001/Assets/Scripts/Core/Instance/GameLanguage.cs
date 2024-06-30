using Game.Cfg;
using System.Collections;
using UnityEngine;

namespace Game.Core
{
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
    }
}