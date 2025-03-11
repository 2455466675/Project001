using Game.UI;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class BattlePointItem : NavigationItem
	{
        public int UnitId { get; private set; }
        public override bool IsValid => CheckValid();

        public Transform actorNode;

        public void SetUnitId(int unitId) 
        {
            UnitId = unitId;
        }

        private bool CheckValid()
        {
            BattleUnit unit = GameWorld.Instance.GetComponent<SystemComponent>().BattleComponent.GetUnit(UnitId);
            Transform tf = unit.ActorComponent.GetBone("center");
            return tf != null;
        }
    }
}

