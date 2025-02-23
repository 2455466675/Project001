using Game.Cfg;
using MVC;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class TextView : DataBaseView
	{
        public ExtendText target;
        public int textId;

        protected override void OnUpdateView()
        {
        }

        public void SetTextById(int textId)
        {
            LanguageItem item = GameCore.Cfg.GetLanguageItem(textId);
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
            SetTextInner(str, GameCore.Cfg.Language.DefaultTextColor);
        }

        public void SetTextByStr(string str, int colorId)
        {
            SetTextInner(str, GameCore.Cfg.GetColorById(colorId));
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

