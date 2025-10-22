using GameFramework.Core;
using UnityEngine;

namespace GameFramework.UI 
{
    public class TextWidget : UIWidget
    {
        [SerializeField]
        private ExtendText m_Text;

        public void SetText(string text) 
        {
            if (this.m_Text == null) 
            {
                return;
            }
            this.m_Text.text = text;
            this.m_Text.color = Utility.Color.DefaultColor;
        }

        public void SetText(int textId) 
        {
            var textItem = Game.GetModule<ConfigManager>().GetTextItem(textId);
            if (textItem == null) 
            {
                SetText(string.Format("error : {0}", textId));
            }
            else
            {
                this.m_Text.text = textItem.Text;
                this.m_Text.color = textItem.Color;
            }
        }

        private void OnValidate()
        {
            if (this.m_Text == null) 
            {
                m_Text = GetComponent<ExtendText>();
            }
        }
    }
}