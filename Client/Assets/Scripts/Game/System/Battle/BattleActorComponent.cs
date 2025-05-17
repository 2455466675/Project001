using UnityEngine;

namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleActorComponent : ActorComponent
    {
        protected override Transform GetActorNode()
        {
            BattleNodeComponent bnc = GetComponent<BattleNodeComponent>();
            return bnc.GetActorNode();    
        }

        public void Clear() 
        {
            RecycleActor();
        }
    }
}
