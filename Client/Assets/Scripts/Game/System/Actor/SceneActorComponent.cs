using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class SceneActorComponent : ActorComponent
    {
        protected override Transform GetActorNode()
        {
            return Game.System.ActorManager.GetScentUnitContainer();
        }
    }
}
