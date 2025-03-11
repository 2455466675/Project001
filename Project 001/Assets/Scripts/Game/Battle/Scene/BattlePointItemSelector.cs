using Game.UI;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class BattlePointItemSelector : NavigationItemSelector
	{
        public BattlePointItem point;

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

            if (point == null)
            {
                return;
            }

            if (state)
            {
                BattleUnit unit = GameWorld.Instance.GetComponent<SystemComponent>().BattleComponent.GetUnit(point.UnitId);
                Transform tf = unit.ActorComponent.GetBone("center");
                if (tf == null) 
                {
                    finger.SetActive(false);
                }
                else
                {                    
                    finger.transform.SetPositionAndRotation(tf.position + (Vector3)offest, Quaternion.AngleAxis(flip ? 180f : 0f, Vector3.up));
                    finger.SetActive(true);
                }
            }
            else
            {
                finger.SetActive(false);
            }
        }
    }
}

