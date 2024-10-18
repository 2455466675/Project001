using Navigation;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class FightPointItemSelector : GuidableItemSelector
	{
        public Actor actor;

        public GameObject finger;
        public Vector2 offest;
        public bool flip;

        public override void OnSelect()
        {
            Finger(true);
        }

        public override void OnDeselect()
        {
            Finger(false);
        }

        public override void OnOutFocus()
        {
            base.OnOutFocus();
        }

        private void Finger(bool state)
        {
            if (finger == null)
            {
                return;
            }

            if (state)
            {
                Transform point = actor.GetBone("center");
                finger.transform.localPosition = point.localPosition + new Vector3(offest.x, offest.y, 0);
                finger.transform.rotation = Quaternion.AngleAxis(flip ? 180f : 0f, Vector3.up);
                finger.SetActive(true);
            }
            else
            {
                finger.SetActive(false);
            }
        }
    }
}

