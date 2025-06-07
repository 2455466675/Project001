using DG.Tweening;
using UnityEngine;

namespace Game.GSystem 
{
    public class AppearMoveAction : ActionItem<AppearMoveActionCommand>
    {
        public enum PosType 
        {
            Local,
            World,
        }

        public PosType posType;
        public Ease ease;
        public Vector3 startPos;
        public Vector3 endPos;
        public float moveTime;
    }

    public class AppearMoveActionCommand : ActionCommand<AppearMoveAction> 
    {
        protected override void OnExecute()
        {
            AppearMoveAction item = Item;
            Actor actor = Player.Actor;
            //actor.transform.localPosition = item.startPos;
            //actor.transform.DOLocalMove(item.endPos, item.moveTime).SetEase(item.ease);
            //actor.animator.Play("attack_002");
        }
    }
}