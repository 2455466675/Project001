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
            return GameObject.FindWithTag("CharacterContainer").transform;
        }
    }
}
