using UnityEngine;

namespace Game.System
{
    public class MoveAction : ActionItem<MoveActionCommand>
    {
        public float speedRate;
        public Vector2 direction;
    }

    public class MoveActionCommand : ActionCommand<MoveAction> 
    {
        protected override void OnExecute()
        {
            Actor actor = Player.Actor;
            if (actor == null || actor.Rigidbody2D == null) 
            {
                return;
            }
            float speed = 0f;
            actor.Rigidbody2D.velocity = (1f + speed) * Item.speedRate * Item.direction;
        }
    }
}