using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class SliderView : View
    {
        [SerializeField]
        private Slider slider;

        public void SetValue(float value) 
        {
            if (slider == null) 
            {
                return;
            }
            slider.value = value;                
        }

        public void SetValue(int arg1, int arg2) 
        {
            if (slider == null)
            {
                return;
            }
            float value = Mathf.Clamp01(arg1 * 1f / arg2);
            slider.value = value;
        }

        private void OnValidate()
        {
            if (slider == null) 
            {
                slider = GetComponent<Slider>();
            }
        }
    }
}
