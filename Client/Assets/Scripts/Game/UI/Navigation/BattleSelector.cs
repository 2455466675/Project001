using Game.System;
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

        [SerializeField]
        private BattleActorView actorView;

        [SerializeField]
        private Vector2 offest;

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

            if (state) 
            {
                Transform bone = actorView.GetBone(ActorBoneDefine.Center);
                Vector3 bonePos = transform.InverseTransformPoint(bone.position);

                finger.transform.localPosition = bonePos + (Vector3)offest;
                finger.SetActive(true);
            }
            else
            {
                finger.SetActive(false);                
            }
        }
    }
}
