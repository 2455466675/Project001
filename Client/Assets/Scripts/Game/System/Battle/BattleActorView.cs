using Game.UI;
using UnityEngine;

namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleActorView : View
    {
        private Actor actor;

        public void SetActor(Actor actor) 
        {
            this.actor = actor;
        }

        public Transform GetBone(string boneName) 
        {
            if (actor == null) 
            {
                return transform;
            }

            return actor.GetBone(boneName);
        }
    }
}
