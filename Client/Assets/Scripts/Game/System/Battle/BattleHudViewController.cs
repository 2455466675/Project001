using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GSystem 
{
    public class BattleHudViewController : ViewController
    {
        [SerializeField]
        private Transform top;
        [SerializeField]
        private Transform bottom;

        public void UpdateHudPos(Transform topAnchor, Transform bottomAnchor) 
        {
            if (top != null && topAnchor != null) 
            {
                top.localPosition = top.parent.InverseTransformPoint(topAnchor.position);
            }

            if (bottom != null && bottomAnchor != null) 
            {
                bottom.localPosition = bottom.parent.InverseTransformPoint(bottomAnchor.position);
            }
        }
    }
}