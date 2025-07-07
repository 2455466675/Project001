using UnityEngine;

namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleNodeComponent : UnitComponent
    {
        private BattleNavigationItem node;

        public void Attach(BattleNavigationItem node) 
        {
            this.node = node;

            var bac = GetComponent<BattleActorComponent>();
            bac.OnRefreshActorEvent += OnRefreshActor;
        }

        public void Clear() 
        {
            var bac = GetComponent<BattleActorComponent>();
            bac.OnRefreshActorEvent -= OnRefreshActor;

            OnRefreshActor(null);
            this.node = null;            
        }

        public Transform GetActorNode() 
        {
            if (this.node == null) 
            {
                return null;
            }

            if (this.node.TryGetView(out BattleActorView view)) 
            {
                return view.transform;
            }

            return null;
        }

        private void OnRefreshActor(Actor obj)
        {
            if (node == null) 
            {
                return;
            }

            if (node.TryGetView(out BattleActorView view))
            {
                view.SetActor(obj);
            }
        }
    }
}
