using UnityEngine;

namespace Game.GSystem
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
            if (actor == null || actor.Rigidbody == null) 
            {
                return;
            }
            float speed = 0f;
            actor.Rigidbody.velocity = (1f + speed) * Item.speedRate * new Vector3(Item.direction.x, 0, Item.direction.y);
        }
    }
}