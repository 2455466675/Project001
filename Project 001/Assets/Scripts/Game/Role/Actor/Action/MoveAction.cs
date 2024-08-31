using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// ÒÆ¶¯RigidBody2D
    /// </summary>
	public class MoveAction : BaseAction
    {
        public float speed;
        public Vector2 dir;
        public override void Execute(Actor actor)
        {
            actor.rb.velocity = speed * dir.normalized;
        }

        public override void Exit()
        {
            
        }
    }
}

