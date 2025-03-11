using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleActorComponent : ActorComponent
    {
        private BattlePointItem point;

        public void SetBattlePointItem(BattlePointItem point) 
        {
            this.point = point;
        }

        protected override Transform GetActorNode()
        {
            if (point == null) 
            { 
                return null; 
            }
            else
            {
                return point.actorNode;                
            }
        }
    }
}
