using Game.UI;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class BattlePointItemSelector : NavigationItemSelector
	{
        public BattleActorLoader fightActor;

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

            if (fightActor == null || fightActor.actor == null)
            {
                return;
            }

            if (state)
            {
                Transform point = fightActor.actor.GetBone("center");
                finger.transform.SetPositionAndRotation(point.position + (Vector3)offest, Quaternion.AngleAxis(flip ? 180f : 0f, Vector3.up));
                finger.SetActive(true);
            }
            else
            {
                finger.SetActive(false);
            }
        }
    }
}

