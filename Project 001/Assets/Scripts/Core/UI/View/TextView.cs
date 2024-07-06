using Game.Core;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class TextView : View
    {
        public ExtendText target;
        public int textId;

        public override void UpdateView()
        {          
        }

        public void SetTextByTd(int textId)
        {
            LanguageItem? item = GameCore.Language.GetLanguageItem(textId);
            if (!item.HasValue)
            {
                return;
            }
            Color color = item.Value.color;
            SetText(item.Value.text, color);     
        }

        public void SetTextByStr(string str)
        {
            SetText(str, Color.black);
        }

        private void SetText(string text, Color color)
        {
            if (target == null) return;
            target.text = text;
            target.color = color;
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            target = GetComponent<ExtendText>();
        }
#endif
    }
}