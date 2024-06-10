using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class TextView : View
    {
        public TextMeshProUGUI target;
        public int textId;

        public override void UpdateView()
        {          
        }

        public void SetTextByTd(int textId)
        {
            SetText(GameCore.Language.GetTextById(textId));     
        }

        public void SetTextByStr(string str)
        {
            SetText(str);
        }

        private void SetText(string text)
        {
            if (target == null) return;
            target.text = text;
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            target = GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}