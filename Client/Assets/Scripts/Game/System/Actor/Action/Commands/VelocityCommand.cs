using UnityEngine;

namespace Game.GSystem
{
    public class VelocityCommand : ActionCommand<VelocityCommandExecutor>
    {
        public float speedRate;
        public Vector2 direction;
    }

    public class VelocityCommandExecutor : ActionCommandExecutor<VelocityCommand> 
    {
        protected override void OnExecute()
        {
            Actor actor = Player.Actor;
            if (actor == null || actor.Rigidbody == null) 
            {
                return;
            }
            float speed = 0f;
            actor.Rigidbody.velocity = (1f + speed) * Command.speedRate * new Vector3(Command.direction.x, 0, Command.direction.y);
        }
    }
}