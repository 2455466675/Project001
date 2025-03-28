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
            if (slider != null) 
            {
                slider.value = value;                
            }
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
