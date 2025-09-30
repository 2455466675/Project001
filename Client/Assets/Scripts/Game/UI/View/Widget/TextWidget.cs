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