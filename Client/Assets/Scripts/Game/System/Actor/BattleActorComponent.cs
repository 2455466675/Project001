using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleActorComponent : ActorComponent
    {
        private Transform node;

        public void SetNode(Transform node) 
        {
            this.node = node;
        }

        protected override Transform GetActorNode()
        {
            return node;    
        }
    }
}
