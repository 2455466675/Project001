using Navigation;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NormalSelector : NavigationItemSelector
    {
        [SerializeField]
        private GameObject outFocus;
        [SerializeField]
        private GameObject finger;

        public override void OnSelect()
        {
            SetOutFocusState(false);
            SetFingerState(true);
        }

        public override void OnDeselect()
        {
            SetOutFocusState(false);
            SetFingerState(false);
        }

        public override void OnOutFocus()
        {
            SetOutFocusState(true);
            SetFingerState(false);
        }

        private void SetOutFocusState(bool state) 
        {
            if (outFocus == null) 
            {
                return;
            }
            outFocus.SetActive(state);
        }
        private void SetFingerState(bool state)
        {
            if (finger == null)
            {
                return;
            }
            finger.SetActive(state);
        }
    }
}
