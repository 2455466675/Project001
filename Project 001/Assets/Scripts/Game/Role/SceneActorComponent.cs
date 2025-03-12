using Game.Core;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class SceneActorComponent : ActorComponent
    {
        protected override Transform GetActorNode()
        {
            return World.GetComponent<SceneComponent>().CharacterContainer;
        }
    }
}
