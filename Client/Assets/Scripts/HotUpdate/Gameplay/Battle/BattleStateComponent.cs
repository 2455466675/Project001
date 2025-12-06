using GameFramework.Core;
using GameFramework.Featrue;

namespace GameFramework.Gameplay
{
    public enum BattleUnitState
    {
        Prime,
        Pause,
        Ready,
        Action,
    }

    public class ComponentStateBase : StateItemBase, IGetComponent
    {
        private readonly Featrue.Component component;

        public ComponentStateBase(Component component)
        {
            this.component = component;
        }

        public T GetComponent<T>() where T : Featrue.Component
        {
            if (component == null)
            {
                return default;            
            }
            else
            {
                return component.GetComponent<T>();
            }
        }
    }

    public class Startup2Prime : StateTriggerBase<PrimeState>
    {
        public override bool Check(IBlackboard blackboard)
        {
            return true;
        }
    }

    public class Prime2Ready : StateTriggerBase<ReadyState>
    {
        public override bool Check(IBlackboard blackboard)
        {
            int sp1 = blackboard.GetBlackboardIntValue(DataKey.SP_1);
            int sp2 = blackboard.GetBlackboardIntValue(DataKey.SP_2);
            return sp2 >= sp1;
        }
    }

    public class Prime2Pause : StateTriggerBase<PauseState>
    {
        public override bool Check(IBlackboard blackboard)
        {
            int battleId = blackboard.GetBlackboardIntValue(DataKey.BattleId);
            int actioning = Game.GetSystem<BattleSystem>().FlowManager.Actioning;

            return actioning > 0 && actioning != battleId;
        }
    }

    public class Ready2Action : StateTriggerBase<ActionState>
    {
        public override bool Check(IBlackboard blackboard)
        {
            int battleId = blackboard.GetBlackboardIntValue(DataKey.BattleId);
            int actioning = Game.GetSystem<BattleSystem>().FlowManager.Actioning;

            return actioning > 0 && actioning == battleId;
        }
    }

    public class Ready2Pause : StateTriggerBase<PauseState>
    {
        public override bool Check(IBlackboard blackboard)
        {
            int battleId = blackboard.GetBlackboardIntValue(DataKey.BattleId);
            int actioning = Game.GetSystem<BattleSystem>().FlowManager.Actioning;

            return actioning > 0 && actioning != battleId;
        }
    }

    public class Pause2Prime : StateTriggerBase<PrimeState>
    {
        public override bool Check(IBlackboard blackboard)
        {
            int actioning = Game.GetSystem<BattleSystem>().FlowManager.Actioning;
            int sp1 = blackboard.GetBlackboardIntValue(DataKey.SP_1);
            int sp2 = blackboard.GetBlackboardIntValue(DataKey.SP_2);
            return actioning == 0 && sp2 < sp1;
        }
    }

    public class Pause2Ready : StateTriggerBase<ReadyState>
    {
        public override bool Check(IBlackboard blackboard)
        {
            int actioning = Game.GetSystem<BattleSystem>().FlowManager.Actioning;
            int sp1 = blackboard.GetBlackboardIntValue(DataKey.SP_1);
            int sp2 = blackboard.GetBlackboardIntValue(DataKey.SP_2);
            return actioning == 0 && sp2 >= sp1;
        }
    }

    public class Action2Prime : StateTriggerBase<PrimeState>
    {
        public override bool Check(IBlackboard blackboard)
        {
            return false;
        }
    }

    public class StartupState : ComponentStateBase
    {
        public StartupState(Component component) : base(component)
        {
            AddTrigger(new Startup2Prime());
        }

        protected override void OnEnter()
        {
            var buc = GetComponent<BattleUnitComponent>();
            SetBlackboardValue(DataKey.BattleId, buc.BattleId);

            var ac = GetComponent<AttributeComponent>();
            int sp1 = ac.GetAttributeValue(AttributeDefine.SP_1);
            int sp2 = ac.GetAttributeValue(AttributeDefine.SP_2);

            SetBlackboardValue(DataKey.SP_1, sp1);
            SetBlackboardValue(DataKey.SP_2, sp2);

            var bmc = GetComponent<BattleMotorComponent>();
            bmc.StartUp();
        }
    }

    public class PrimeState : ComponentStateBase
    {
        public PrimeState(Component component) : base(component)
        {
            AddTrigger(new Prime2Ready());
            AddTrigger(new Prime2Pause());
        }

        protected override void OnEnter()
        {
            MDebug.Log("PrimeState OnEnter");
        }

        protected override void OnTick()
        {
            var ac = GetComponent<AttributeComponent>();
            int sp1 = ac.GetAttributeValue(AttributeDefine.SP_1);
            int sp2 = ac.GetAttributeValue(AttributeDefine.SP_2);
            int spr = ac.GetFinalValue(AttributeDefine.SP_RATE);

            int v = Utility.Math.Min(sp1, sp2 + spr);
            ac.SetAttributeValue(AttributeDefine.SP_2, v);

            SetBlackboardValue(DataKey.SP_1, sp1);
            SetBlackboardValue(DataKey.SP_2, v);
        }
    }

    public class PauseState : ComponentStateBase
    {
        public PauseState(Component component) : base(component)
        {
            AddTrigger(new Pause2Prime());
            AddTrigger(new Pause2Ready());
        }

        protected override void OnEnter()
        {
            MDebug.Log("PauseState OnEnter");
        }
    }

    public class ReadyState : ComponentStateBase
    {
        public ReadyState(Component component) : base(component)
        {
            AddTrigger(new Ready2Action());
            AddTrigger(new Ready2Pause());
        }

        protected override void OnEnter()
        {
            int battleId = GetBlackboardIntValue(DataKey.BattleId);
            Game.GetSystem<BattleSystem>().FlowManager.EnqueueReady(battleId);
        }

        protected override void OnExit()
        {
            int battleId = GetBlackboardIntValue(DataKey.BattleId);
            Game.GetSystem<BattleSystem>().FlowManager.RemoveReady(battleId);
        }
    }

    public class ActionState : ComponentStateBase
    {
        public ActionState(Component component) : base(component)
        {
            AddTrigger(new Action2Prime());
        }

        protected override void OnEnter()
        {
            MDebug.Log("ActionState OnEnter");

            int battleId = GetBlackboardIntValue(DataKey.BattleId);
            Game.GetSystem<BattleSystem>().FlowManager.DoAction(battleId);
        }
    }

    public class BattleStateComponent : Featrue.Component
    {
        private GameStateMachine m_Machine;

        protected override void OnInit()
        {
            m_Machine = new GameStateMachine();

            m_Machine.AddState(new StartupState(this));
            m_Machine.AddState(new PrimeState(this));
            m_Machine.AddState(new PauseState(this));
            m_Machine.AddState(new ReadyState(this));
            m_Machine.AddState(new ActionState(this));
        }

        public void StartUp()
        {
            m_Machine.Run<StartupState>();
        }

        public void Tick()
        {
            m_Machine.Tick();
        }
    }
}
