using Cysharp.Threading.Tasks;
using GameFramework.Core;
using GameFramework.Featrue;
using System;
using System.Collections.Generic;
using UnityEngine;

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

            DataModel param = new DataModel();
            param.SetValue(DataKey.BattleId, battleId);
            executor = new BattleExecutor(param);
            MoveNext(null);
        }

        public void ActionOver()
        {
            Entity entity = Game.GetSystem<BattleSystem>().GetBattleUnit(Actioning);
            if (entity == null)
            {
                return;
            }

            Game.Event.Publish(new NavigateClearEventArgs());

            var ac = entity.GetComponent<AttributeComponent>();
            ac.SetAttributeValue(AttributeDefine.SP_2, 0);

            Actioning = 0;
        }

        public void MoveNext(DataModel param = null)
        {
            executor?.MoveNext(param);
        }

        public void MoveBack()
        {
            executor?.MoveBack();
        }
    }

    public class BattleExecutor : GameCommand
    {
        protected DataModel pipelineParam;

        public BattleExecutor(DataModel param)
        {
            pipelineParam = new DataModel();
            pipelineParam.AddRange(param);
            BattleExecutorCommand cammand = new StartupCommand();
            cammand.SetParam(pipelineParam);
            Push(cammand);
        }

        public void MoveNext(DataModel param)
        {
            pipelineParam.AddRange(param);

            if (TryPeek<BattleExecutorCommand>(out var cammand))
            {
                cammand = cammand.Next();
                if (cammand != null)
                {
                    cammand.SetParam(pipelineParam);
                    Push(cammand);
                }
            }
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

                if (TryPeek<BattleExecutorCommand>(out var cammand))
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

    public class BattleExecutorCommand : GameCommand
    {
        protected DataModel pipelineParam;
        public virtual bool IsAnchor => false;

        public bool IsStartingPoint { get; set; }

        protected override bool CheckLocked()
        {
            return IsStartingPoint;
        }

        public void SetParam(DataModel param)
        {
            this.pipelineParam = param;
        }

        public virtual BattleExecutorCommand Next()
        {
            return null;
        }
    }

    public struct NavigateBattleGridArgs : IGameEventArgs
    {
        public int coordX;
        public int coordY;

        public NavigateBattleGridType type;
    }

    public class StartupCommand : BattleExecutorCommand
    {
        public override BattleExecutorCommand Next()
        {
            BattleExecutorCommand cammand = new SelectMoveTargetPosition();
            cammand.IsStartingPoint = true;
            return cammand;
        }
    }

    public class SelectMoveTargetPosition : BattleExecutorCommand
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

        protected override void OnSink()
        {
            Game.Event.Publish(new NavigateBackEventArgs());
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
            Game.GetModule<CameraManager>().LookAt(BattleUtils.Coord2Pos(btc.CoordX, btc.CoordY));

            Game.Event.Publish(new NavigateBattleGridArgs() { coordX = btc.CoordX, coordY = btc.CoordY, type = NavigateBattleGridType.SelectMovePosition });

            return true;
        }

        public override BattleExecutorCommand Next()
        {
            return new MoveToTargetPosition();
        }
    }

    public class MoveToTargetPosition : BattleExecutorCommand
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
            var coord = BattleUtils.Index2Coord(index);

            int targetX = coord.x;
            int targetY = coord.y;

            Game.GetModule<CameraManager>().LookAt(BattleUtils.Coord2Pos(targetX, targetY));

            var bfc = entity.GetComponent<BattleTransformComponent>();
            var bmc = entity.GetComponent<BattleMotorComponent>();

            var path = BattleUtils.AStarPath(bfc.CoordX, bfc.CoordY, targetX, targetY, H_Value, CheckIndexValid);

            coordX = bfc.CoordX;
            coordY = bfc.CoordY;
            dirX = bfc.DirX;
            dirY = bfc.DirY;
            
            await bmc.MoveAsync(path);

            Game.GetSystem<BattleSystem>().FlowManager.MoveNext();
        }

        private bool CheckIndexValid(int x, int y)
        {
            if (x < 0 || x >= BattleUtils.ColCount || y < 0 || y >= BattleUtils.RowCount)
            {
                return false;
            }

            TileItem item = BattleUtils.GetTile(x, y);
            if (item == null)
            {
                return false;
            }

            return item.Passable();
        }

        private int H_Value(int startX, int startY, int targetX, int targetY)
        {
            return Utility.Math.Abs(startX - targetX) + Utility.Math.Abs(startY - targetY);
        }

        public override BattleExecutorCommand Next()
        {
            return new SelectBattleAction();
        }
    }

    public class SelectBattleAction : BattleExecutorCommand
    {
        public override bool IsAnchor => true;

        protected override bool OnPush()
        {
            Game.Event.Publish(new NavigateEventArgs() { navigationDefine = NavigationDefine.BattleActionList });

            return true;
        }

        protected override void OnPop()
        {
            Game.Event.Publish(new NavigateBackEventArgs());
        }

        public override BattleExecutorCommand Next()
        {
            return new SelectBattleAction2();
        }
    }

    public class SelectBattleAction2 : BattleExecutorCommand
    {
        public override bool IsAnchor => true;

        protected override bool OnPush()
        {
            Game.Event.Publish(new NavigateEventArgs() { navigationDefine = NavigationDefine.BattleActionList2 });

            return true;
        }

        protected override void OnPop()
        {
            Game.Event.Publish(new NavigateBackEventArgs());
        }

        protected override void OnSink()
        {
            Game.Event.Publish(new PanelAlphaEventArgs() { panel = PanelDefine.BattleActionPanel, alpha = 0f });
            Game.Event.Publish(new PanelAlphaEventArgs() { panel = PanelDefine.BattleActionPanel2, alpha = 0f });
        }

        protected override bool OnRise()
        {
            Game.Event.Publish(new PanelAlphaEventArgs() { panel = PanelDefine.BattleActionPanel, alpha = 1f });
            Game.Event.Publish(new PanelAlphaEventArgs() { panel = PanelDefine.BattleActionPanel2, alpha = 1f });
            return true;
        }

        public override BattleExecutorCommand Next()
        {
            return new SelectEffectArea();
        }
    }

    public class SelectEffectArea : BattleExecutorCommand
    {
        public override bool IsAnchor => true;

        protected override bool OnPush()
        {
            return Execute();
        }

        protected override void OnPop()
        {
            Game.Event.Publish(new NavigateBackEventArgs());
        }

        protected override void OnSink()
        {
            Game.Event.Publish(new NavigateBackEventArgs());
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
            Game.GetModule<CameraManager>().LookAt(BattleUtils.Coord2Pos(btc.CoordX, btc.CoordY));

            Game.Event.Publish(new NavigateBattleGridArgs() { coordX = btc.CoordX, coordY = btc.CoordY, type = NavigateBattleGridType.SelectEffectArea });

            return true;
        }

        public override BattleExecutorCommand Next()
        {          
            return new EffectCommand();
        }
    }

    public class EffectCommand : BattleExecutorCommand
    {
        protected override bool OnPush()
        {
            Play().Forget();

            return true;
        }

        private async UniTask Play()
        {
            int index = pipelineParam.GetIntValue(DataKey.Index);
            var coord = BattleUtils.Index2Coord(index);

            List<Vector2Int> area = new List<Vector2Int>();

            List<Vector2Int> areaPoints = new List<Vector2Int>()
            {
                 new Vector2Int(0, 0),
                 new Vector2Int(0, 1),
                 new Vector2Int(1, 0),
                 new Vector2Int(-1, 0),
                 new Vector2Int(0, -1),
            };

            foreach (var item in areaPoints)
            {
                area.Add(coord + item);
            }

            int battleId = pipelineParam.GetIntValue(DataKey.BattleId);

            Entity entity = Game.GetSystem<BattleSystem>().GetBattleUnit(battleId);
            var ac = entity.GetComponent<ActorComponent>();

            ActionData actionData = new ActionData();
            actionData.actor = ac;
            actionData.points = areaPoints;
            actionData.direction = Utility.Math.Random(1, 5);

            var handle = Game.GetModule<ActionManager>().Play("TestSkill1001", actionData);

            await handle.Task;

            Game.GetSystem<BattleSystem>().FlowManager.ActionOver();
        }
    }
}
