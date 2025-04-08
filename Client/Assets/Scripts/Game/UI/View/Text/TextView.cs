using Game.Config;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class TextView : View
    {
        [SerializeField]
        private ExtendText target;

        public void SetTextById(int textId)
        {
            LanguageItem item = Game.Config.GetLanguageItem(textId);
            if (item == null)
            {
                SetTextInner(string.Empty, default);
            }
            else
            {
                SetTextInner(item.TextValue, item.ColorValue);
            }
        }

        public void SetTextByStr(string str)
        {
            SetTextInner(str, GameLanguage.DefaultTextColor);
        }

        public void SetTextByStr(string str, int colorId)
        {
            SetTextInner(str, Game.Config.GetColorById(colorId));
        }

        public void SetTextByStr(string str, Color color)
        {
            SetTextInner(str, color);
        }

        private void SetTextInner(string text, Color color)
        {
            if (target == null) return;
            target.text = text;
            target.color = color;
        }

#if UNITY_EDITOR
        public virtual void OnValidate()
        {
            target = GetComponent<ExtendText>();
        }
#endif
    }
}
