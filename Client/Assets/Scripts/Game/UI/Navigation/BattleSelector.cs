using Navigation;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleSelector : NavigationItemSelector
    {
        [SerializeField]
        private GameObject finger;

        public override void OnSelect()
        {
            SetFingerState(true);
        }

        public override void OnDeselect()
        {
            SetFingerState(false);
        }

        public override void OnOutFocus()
        {

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
