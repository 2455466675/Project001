using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.UI
{
    public class SliderWidget : UIWidget
    {
        [SerializeField]
        private Slider m_Slider;

        public void SetValue(float value) 
        {
            if (m_Slider != null)
            {
                m_Slider.value = value;
            }
        }

        private void OnValidate()
        {
            if (this.m_Slider == null)
            {
                m_Slider = GetComponent<Slider>();
            }
        }
    }
}
