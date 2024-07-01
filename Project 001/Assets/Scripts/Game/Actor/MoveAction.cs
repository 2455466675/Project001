using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// ÒÆ¶¯RigidBody2D
    /// </summary>
	public class MoveAction : BaseAction
    {
        public float speed;
        public Vector2 dir;
        public override void Execute()
        {
            actor.rb.velocity = speed * dir.normalized;
        }

        public override void Exit()
        {
            
        }
    }
}

