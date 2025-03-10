using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// ÒÆ¶¯RigidBody2D
    /// </summary>
	public class MoveAction : ActorBaseAction
    {
        public float speed;
        public Vector2 dir;
        public override void Execute(Actor actor, params object[] actionArgs)
        {
            actor.rb.velocity = speed * dir.normalized;
        }
    }
}

