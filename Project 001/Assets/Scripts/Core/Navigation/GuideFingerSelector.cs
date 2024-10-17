using UnityEngine;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
	public class GuideFingerSelector : GuidableItemSelector
	{
        public GameObject outFocus;
        public GameObject finger;

        private void Awake()
        {
            OutFocus(false);
            Finger(false);
        }

        public override void OnSelect()
        {
            OutFocus(false);
            Finger(true);
        }

        public override void OnDeselect()
        {
            OutFocus(false);
            Finger(false);
        }

        public override void OnOutFocus()
        {
            OutFocus(true);
            Finger(false);
        }

        private void Finger(bool state)
        {
            if (finger == null)
            {
                return;
            }
            finger.SetActive(state);
        }

        private void OutFocus(bool state)
        {
            if (outFocus == null)
            {
                return;
            }
            outFocus.SetActive(state);
        }
    }
}

