using Game.UI.Input;
using Game.UI;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Game.GSystem 
{
    #region

    public enum PhaseType
    {
        Prime,
        Pause,
        Ready,        
        Action,        
    }

    public abstract class BattleUnitPhaseBase
    {
        public abstract PhaseType PhaseType { get; }

        private BattleBehaviorComponent component;

        public BattleUnitPhaseBase(BattleBehaviorComponent component) 
        {
            this.component = component;
        }

        public void Enter() 
        {
            OnEnter();
        }
        public void Tick() 
        {        
            OnTick();
        }
        public void Exit() 
        {
            OnExit();
        }

        protected T GetComponent<T>() where T : UnitComponent
        {
            return this.component.GetComponent<T>();
        }

        protected abstract void OnEnter();
        protected abstract void OnTick();
        protected abstract void OnExit();
    }

    public class PrimePhase : BattleUnitPhaseBase
    {
        public PrimePhase(BattleBehaviorComponent component) : base(component)
        {
        }

        public override PhaseType PhaseType => PhaseType.Prime;

        protected override void OnEnter()
        {
        }

        protected override void OnExit()
        {
        }

        protected override void OnTick()
        {
            BattleAttributeComponent attributeComponent = GetComponent<BattleAttributeComponent>();
            int sp1 = attributeComponent.GetAttributeValue(AttributeDefine.SP_1);
            int sp2 = attributeComponent.GetAttributeValue(AttributeDefine.SP_2);
            int spr = attributeComponent.GetFinalValue(AttributeDefine.SP_RATE);

            sp2 = GameMathf.Min(sp1, sp2 + spr);
            attributeComponent.SetAttributeValue(AttributeDefine.SP_2, sp2);
        }
    }

    public class PausePhase : BattleUnitPhaseBase
    {
        public PausePhase(BattleBehaviorComponent component) : base(component)
        {
        }

        public override PhaseType PhaseType => PhaseType.Pause;

        protected override void OnEnter()
        {
        }

        protected override void OnExit()
        {
        }

        protected override void OnTick()
        {
        }
    }

    public class ReadyPhase : BattleUnitPhaseBase
    {
        public ReadyPhase(BattleBehaviorComponent component) : base(component)
        {
        }

        public override PhaseType PhaseType => PhaseType.Ready;

        protected override void OnEnter()
        {
            GetComponent<BattleBehaviorComponent>().EnterReady();
        }

        protected override void OnExit()
        {
        }

        protected override void OnTick()
        {
        }
    }

    public class ActionPhase : BattleUnitPhaseBase
    {
        public ActionPhase(BattleBehaviorComponent component) : base(component)
        {
        }

        public override PhaseType PhaseType => PhaseType.Action;

        protected override void OnEnter()
        {
            GetComponent<BattleBehaviorComponent>().IsActioning = true;

            //var btc = GetComponent<BattleTransformComponent>();
            //var index = BattleUtil.IndexToIndex(btc.pX, btc.pY);
            //Game.UI.Navigate(NavigationListDefine.Battle_Grid, ModuleType.Battle, new int[] { index });

            BattleInputModule.Instance.Push(new SelectMoveTargetCammand());

            /* Command
             * SelectMoveTargetCmd 
             * UnitMoveCmd  undo -> 
             * 
            */

        }

        protected override void OnExit()
        {
            GetComponent<BattleBehaviorComponent>().ExitAction();

            BattleAttributeComponent attributeComponent = GetComponent<BattleAttributeComponent>();
            attributeComponent.SetAttributeValue(AttributeDefine.SP_2, 0);
        }

        protected override void OnTick()
        {
        }
    }

    #endregion

    #region

    public abstract class PhaseTriggerCondition
    {
        public abstract bool Check(BattleBehaviorComponent component);    
    }

    public class Prime2Ready : PhaseTriggerCondition
    {
        public override bool Check(BattleBehaviorComponent component)
        {
            BattleAttributeComponent attributeComponent = component.GetComponent<BattleAttributeComponent>();
            int sp1 = attributeComponent.GetAttributeValue(AttributeDefine.SP_1);
            int sp2 = attributeComponent.GetAttributeValue(AttributeDefine.SP_2);

            return sp2 >= sp1;
        }
    }

    public class Prime2Pause : PhaseTriggerCondition
    {
        public override bool Check(BattleBehaviorComponent component)
        {
            int id = component.ActioningBattleId();

            return id >= 0;
        }
    }

    public class Ready2Action : PhaseTriggerCondition
    {
        public override bool Check(BattleBehaviorComponent component)
        {
            int id = component.ActioningBattleId();
            int bId = component.GetParent<BattleUnit>().BattleId;

            return id == bId;
        }
    }

    public class Ready2Pause : PhaseTriggerCondition
    {
        public override bool Check(BattleBehaviorComponent component)
        {
            int id = component.ActioningBattleId();
            int bId = component.GetParent<BattleUnit>().BattleId;

            return id != bId;
        }
    }

    public class Pause2Prime : PhaseTriggerCondition
    {
        public override bool Check(BattleBehaviorComponent component)
        {
            int id = component.ActioningBattleId();

            BattleAttributeComponent attributeComponent = component.GetComponent<BattleAttributeComponent>();
            int sp1 = attributeComponent.GetAttributeValue(AttributeDefine.SP_1);
            int sp2 = attributeComponent.GetAttributeValue(AttributeDefine.SP_2);

            return id == -1 && sp2 < sp1;
        }
    }

    public class Pause2Ready : PhaseTriggerCondition
    {
        public override bool Check(BattleBehaviorComponent component)
        {
            int id = component.ActioningBattleId();

            BattleAttributeComponent attributeComponent = component.GetComponent<BattleAttributeComponent>();
            int sp1 = attributeComponent.GetAttributeValue(AttributeDefine.SP_1);
            int sp2 = attributeComponent.GetAttributeValue(AttributeDefine.SP_2);

            return id == -1 && sp2 >= sp1;
        }
    }

    public class Action2Prime : PhaseTriggerCondition
    {
        public override bool Check(BattleBehaviorComponent component)
        {         
            return !component.IsActioning;
        }
    }

    #endregion

    public class PhaseTriggerItem
    {
        public PhaseType Source { get; private set; }
        public PhaseType Target { get; private set; }
        private PhaseTriggerCondition condition;

        public PhaseTriggerItem(PhaseType source, PhaseType target, PhaseTriggerCondition condition)
        {
            Source = source;
            Target = target;
            this.condition = condition;
        }

        public bool IsTriggered(BattleBehaviorComponent component) 
        { 
            return condition.Check(component); 
        }
    }

    public class BattleBehaviorComponent : UnitComponent, IAwakeComponent
    {
        private BattleTurnController turnController;

        private List<BattleUnitPhaseBase> phases;
        private Dictionary<PhaseType, List<PhaseTriggerItem>> triggers;

        private BattleUnitPhaseBase currPhase;

        public bool IsActioning { get; set; }

        public int step;

        public void Awake() 
        {
            phases = new List<BattleUnitPhaseBase>()
            {
                new PrimePhase(this),
                new PausePhase(this),
                new ReadyPhase(this),
                new ActionPhase(this),
            };

            triggers = new Dictionary<PhaseType, List<PhaseTriggerItem>>();

            AddTrigger(new PhaseTriggerItem(PhaseType.Prime, PhaseType.Pause, new Prime2Pause()));
            AddTrigger(new PhaseTriggerItem(PhaseType.Prime, PhaseType.Ready, new Prime2Ready()));
            AddTrigger(new PhaseTriggerItem(PhaseType.Ready, PhaseType.Pause, new Ready2Pause()));
            AddTrigger(new PhaseTriggerItem(PhaseType.Ready, PhaseType.Action, new Ready2Action()));
            AddTrigger(new PhaseTriggerItem(PhaseType.Pause, PhaseType.Prime, new Pause2Prime()));
            AddTrigger(new PhaseTriggerItem(PhaseType.Pause, PhaseType.Ready, new Pause2Ready()));
            AddTrigger(new PhaseTriggerItem(PhaseType.Action, PhaseType.Prime, new Action2Prime()));
        }

        private void AddTrigger(PhaseTriggerItem item) 
        {
            if (triggers.TryGetValue(item.Source, out List<PhaseTriggerItem> items)) 
            {
                items.Add(item);
            }
            else
            {
                items = new List<PhaseTriggerItem>();
                items.Add(item);
                triggers.Add(item.Source, items);
            }
        }

        public int ActioningBattleId() 
        {
            return turnController.ActioningBattleId;
        }

        public void EnterReady() 
        {
            int bId = GetParent<BattleUnit>().BattleId;
            turnController.EnterReady(bId);
        }

        public void ExitAction() 
        {
            int bId = GetParent<BattleUnit>().BattleId;
            turnController.ExitAction(bId);
        }

        public void Attach(BattleTurnController turnController) 
        {
            this.turnController = turnController;
        }

        public void StartUp() 
        {
            SwitchTo(PhaseType.Prime);
        }

        public void Tick()
        {
            CheckTrigger();
            currPhase?.Tick();
        }

        private void SwitchTo(PhaseType phaseType) 
        {
            if (currPhase != null && currPhase.PhaseType == phaseType) 
            {
                return;
            }

            BattleUnitPhaseBase phase = phases.Find(p => p.PhaseType == phaseType);
            if (phase == null)
            {
                return;
            }

            currPhase?.Exit();
            currPhase = phase;
            currPhase.Enter();
        }

        private void CheckTrigger() 
        {
            if (currPhase == null)
            {
                return;
            }

            if (triggers.TryGetValue(currPhase.PhaseType, out List<PhaseTriggerItem> items))
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].IsTriggered(this))
                    {
                        SwitchTo(items[i].Target);
                        return;
                    }
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

        private async UniTask AwaitMove(List<Vector2Int> path) 
        {

            var btc = GetComponent<BattleTransformComponent>();
            await btc.MoveAsync(path);

            var index = BattleUtil.IndexToIndex(btc.pX, btc.pY);
            Game.UI.Navigate(NavigationListDefine.Battle_Grid, ModuleType.Battle, new int[] { index });
            step = 1;
        }

        private async UniTask AwaitAction() 
        {
            var handler = GetComponent<ActorComponent>().PlayAction("Skill001");
            await handler.Task;          
            IsActioning = false;
            step = 0;
        }

        protected override void OnDestroyComponent()
        {

        }
    }
}