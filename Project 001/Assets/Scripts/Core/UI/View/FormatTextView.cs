using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class FormatTextView : TextView
	{
        public override void UpdateView()
        {
            if (!IsValid) return;

            string lang = GameCore.Language.GetTextById(textId);
            if (string.IsNullOrEmpty(lang)) return;

            string[] args = new string[DBs.Length];
            for (int i = 0; i < DBs.Length; i++)
            {
                args[i] = GameCore.Language.GetTextById(DBs[i].IntValue);
            }

            string text = string.Format(lang, args);
            SetTextByStr(text);
        }
    }
}

