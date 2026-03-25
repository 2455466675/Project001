using TMPro;
using UnityEngine;

namespace GameFramework.View.UI
{
    public class TextWidget : UIWidget
    {
        [SerializeField]
        private TextMeshProUGUI m_Text;

        public void SetText(string text) 
        {
            if (this.m_Text == null) 
            {
                return;
            }
            this.m_Text.text = text;
        }

        public void SetTextById(string textId) 
        {
            var textItem = Game.Config.GetTextItem(textId);
            if (textItem == null)
            {
                SetText(string.Format("text error : {0}", textId));
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
                m_Text = GetComponent<TextMeshProUGUI>();
            }
        }
    }
}