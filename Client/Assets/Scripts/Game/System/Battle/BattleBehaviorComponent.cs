using Game.UI.Input;
using Game.UI;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Game.GSystem 
{
    public enum BehaviorState
    {
        Prime,
        Ready,
        MoveBuffer,
        MovePlay,
        ActionBuffer,
        ActionPlay,
    }

    public class BattleBehaviorComponent : UnitComponent, IAwakeComponent
    {
        private BattleAttributeComponent attributeComponent;

        public BehaviorState State { get; private set; }

        public void Awake() 
        {
            State = BehaviorState.Prime;
            attributeComponent = GetComponent<BattleAttributeComponent>();
        }

        public void Step() 
        {
            if (State == BehaviorState.Prime) 
            {
                var sp1 = attributeComponent.GetAttributeValue(AttributeDefine.SP_1);
                var sp2 = attributeComponent.GetAttributeValue(AttributeDefine.SP_2);
                var spr = attributeComponent.GetFinalValue(AttributeDefine.SP_RATE);
                                
                sp2 = GameMathf.Min(sp1, sp2 + spr);
                attributeComponent.SetAttributeValue(AttributeDefine.SP_2, sp2);

                if (sp2 >= sp1)
                {
                    State = BehaviorState.Ready;
                }
            }
        }

        public void Move(List<Vector2Int> path) 
        {
            AwaitMove(path).Forget();
        }

        public void Action() 
        {
            AwaitAction().Forget();
        }

        public void AwaitBuffer()
        {
            if (State == BehaviorState.Ready) 
            {
                State = BehaviorState.MoveBuffer;

                var btc = GetComponent<BattleTransformComponent>();
                var index = BattleUtil.IndexToIndex(btc.pX, btc.pY);
                Game.UI.Navigate(NavigationListDefine.Battle_Grid, ModuleType.Battle, new int[] { index });
            }
        }

        private async UniTask AwaitMove(List<Vector2Int> path) 
        {
            State = BehaviorState.MovePlay;
            var btc = GetComponent<BattleTransformComponent>();
            await btc.MoveAsync(path);
            State = BehaviorState.ActionBuffer;

            var index = BattleUtil.IndexToIndex(btc.pX, btc.pY);
            Game.UI.Navigate(NavigationListDefine.Battle_Grid, ModuleType.Battle, new int[] { index });
        }

        private async UniTask AwaitAction() 
        {
            State = BehaviorState.ActionPlay;
            var handler = GetComponent<ActorComponent>().PlayAction("Skill001");
            await handler.Task;
            State = BehaviorState.Prime;
            attributeComponent.SetAttributeValue(AttributeDefine.SP_2, 0);
        }

        protected override void OnDestroyComponent()
        {
            attributeComponent = null;
        }
    }
}