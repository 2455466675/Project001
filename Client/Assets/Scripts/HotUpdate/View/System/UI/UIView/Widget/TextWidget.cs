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
            if (this.m_Text == null)
            {
                return;
            }
            m_Text.text = Game.Config.GetTextById(textId);
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