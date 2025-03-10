using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ActorBaseAction : MonoBehaviour
    {
        public abstract void Execute(Actor actor, params object[] actionArgs);    
    }
}
