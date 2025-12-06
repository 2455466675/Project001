using Cysharp.Threading.Tasks;
using GameFramework.Core;
using GameFramework.Featrue;
using System.Collections.Generic;

namespace GameFramework.Gameplay
{
    public class BattleFlowManager
    {
        public int Actioning { get; set; }

        private List<int> m_AllUnits;
        private List<int> m_ReadyUnits;

        private List<BattleStateComponent> m_Components;

        private BattleExecutor executor;

        public BattleFlowManager()
        {
            m_AllUnits = new List<int>();
            m_ReadyUnits = new List<int>();
            m_Components = new List<BattleStateComponent>();
        }

        public void StartUp()
        {
            foreach (var item in m_Components)
            {
                item.StartUp();
            }
        }

        public void Tick()
        {
            if (Actioning <= 0)
            {
                Actioning = DequeueReady();
            }

            foreach (var item in m_Components)
            {
                item.Tick();
            }
        }

        public void AddUnit(Entity entity)
        {
            var buc = entity.GetComponent<BattleUnitComponent>();
            var bsc = entity.GetComponent<BattleStateComponent>();

            m_AllUnits.Add(buc.BattleId);
            m_Components.Add(bsc);
        }

        public void RemoveUnit(int battleId)
        {
            m_AllUnits.Remove(battleId);
            m_ReadyUnits.Remove(battleId);
        }

        public void EnqueueReady(int battleId)
        {
            m_ReadyUnits.Add(battleId);
        }

        public int DequeueReady()
        {
            if (m_ReadyUnits.Count == 0)
            {
                return 0;
            }
            else
            {
                int r = m_ReadyUnits[0];
                m_ReadyUnits.Remove(0);
                return r;                
            }
        }

        public bool RemoveReady(int battleId)
        {
            return m_ReadyUnits.Remove(battleId);
        }

        public void DoAction(int battleId)
        {
            if (battleId != Actioning)
            {
                return;
            }

            executor = new BattleExecutor();

            DataModel param = new DataModel();
            param.SetValue(DataKey.BattleId, battleId);
            executor.MoveNext(new SelectMoveTargetPosition(), param);
        }

        public void MoveNext(BattleExecutorCammand cammand, DataModel param = null)
        {
            executor?.MoveNext(cammand, param);
        }

        public void MoveBack()
        {
            executor?.MoveBack();
        }
    }

    public class BattleExecutor : GameCammand
    {
        protected DataModel pipelineParam;

        public BattleExecutor()
        {
            pipelineParam = new DataModel();
        }

        public void MoveNext(BattleExecutorCammand cammand, DataModel param)
        {
            pipelineParam.AddRange(param);
            cammand.SetParam(pipelineParam);
            Push(cammand);
        }

        public void MoveBack()
        {
            Pop();

            while (true)
            {
                if (Count <= 0)
                {
                    break;
                }

                if (TryPeek<BattleExecutorCammand>(out var cammand))
                {
                    if (cammand.IsAnchor)
                    {
                        break;
                    }
                    if (cammand.IsLocked)
                    {
                        break;
                    }
                    Pop();
                }
            }
        }
    }

    public class BattleExecutorCammand : GameCammand
    {
        protected DataModel pipelineParam;
        public virtual bool IsAnchor => false;

        public void SetParam(DataModel param)
        {
            this.pipelineParam = param;
        }
    }

    public struct NavigateBattleGridArgs : IGameEventArgs
    {
        public int coordX;
        public int coordY;
    }

    public class SelectMoveTargetPosition : BattleExecutorCammand
    {
        public override bool IsAnchor => true;

        protected override bool OnPush()
        {
            return Execute();
        }

        protected override bool OnRise()
        {
            return Execute();
        }

        private bool Execute()
        {
            int battleId = pipelineParam.GetIntValue(DataKey.BattleId);

            Entity entity = Game.GetSystem<BattleSystem>().GetBattleUnit(battleId);
            if (entity == null)
            {
                return false;
            }

            var btc = entity.GetComponent<BattleTransformComponent>();
            Game.GetModule<CameraManager>().LookAt(Game.GetSystem<BattleSystem>().GridManager.Coord2Pos(btc.CoordX, btc.CoordY));

            Game.Event.Publish(new NavigateBattleGridArgs() { coordX = btc.CoordX, coordY = btc.CoordY });

            return true;
        }
    }

    public class MoveToTargetPosition : BattleExecutorCammand
    {
        public override bool IsAnchor => false;

        private int coordX;
        private int coordY;
        private float dirX;
        private float dirY;

        protected override bool OnPush()
        {
            Execute().Forget();

            return true;
        }

        protected override void OnPop()
        {
            int battleId = pipelineParam.GetIntValue(DataKey.BattleId);
            Entity entity = Game.GetSystem<BattleSystem>().GetBattleUnit(battleId);
            if (entity == null)
            {
                return;
            }

            var bfc = entity.GetComponent<BattleTransformComponent>();
            bfc.SetCoordPosition(coordX, coordY);
            bfc.SetDirection(dirX, dirY);
        }

        private async UniTaskVoid Execute()
        {
            int battleId = pipelineParam.GetIntValue(DataKey.BattleId);

            Entity entity = Game.GetSystem<BattleSystem>().GetBattleUnit(battleId);
            if (entity == null)
            {
                return;
            }

            var bs = Game.GetSystem<BattleSystem>();

            int index = pipelineParam.GetIntValue(DataKey.Index);
            var coord = bs.GridManager.Index2Coord(index);

            int targetX = coord.x;
            int targetY = coord.y;

            Game.GetModule<CameraManager>().LookAt(bs.GridManager.Coord2Pos(targetX, targetY));

            var bfc = entity.GetComponent<BattleTransformComponent>();
            var bmc = entity.GetComponent<BattleMotorComponent>();

            var path = bs.GridManager.AStarPath(bfc.CoordX, bfc.CoordY, targetX, targetY);

            coordX = bfc.CoordX;
            coordY = bfc.CoordY;
            dirX = bfc.DirX;
            dirY = bfc.DirY;
            
            await bmc.MoveAsync(path);

            Game.GetSystem<BattleSystem>().FlowManager.MoveNext(new SelectBattleAction());
        }
    }

    public class SelectBattleAction : BattleExecutorCammand
    {
        public override bool IsAnchor => true;

        protected override bool OnPush()
        {
            Game.Event.Publish(new NavigateEventArgs() { navigationDefine = NavigationDefine.BattleActionList });

            return true;
        }

        protected override void OnPop()
        {
            MDebug.Log("SelectBattleAction OnPop");

            Game.Event.Publish(new NavigateBackEventArgs());
        }
    }

    public class SelectBattleAction2 : BattleExecutorCammand
    {
        public override bool IsAnchor => true;

        protected override bool OnPush()
        {
            Game.Event.Publish(new NavigateEventArgs() { navigationDefine = NavigationDefine.BattleActionList2 });

            return true;
        }

        protected override void OnPop()
        {
            MDebug.Log("SelectBattleAction2 OnPop");
            Game.Event.Publish(new NavigateBackEventArgs());
        }
    }
}
