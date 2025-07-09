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
            var root = Game.System.BattleSystem.GetBattleSceneView<BattleUnitRootView>();
            return root.transform;    
        }

        public void Clear() 
        {
            RecycleActor();
        }
    }
}
