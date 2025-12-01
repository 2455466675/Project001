using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Core;
using GameFramework.Featrue;

namespace GameFramework.Gameplay 
{

    public struct EnterBattleEventArgs : IGameEventArgs
    {
        public bool isEnter;
    }

    [Gameplay]
    public class BattleSystem : IGameplaySystem, IFixedUpdate
    {
        public BattleFlowManager FlowManager { get; private set; }
        public BattleGridManager GridManager { get; private set; }

        private List<Entity> entities;

        public Entity Entity => entities[0];

        private bool isBattling;

        public void OnInit()
        {
            FlowManager = new BattleFlowManager();
            GridManager = new BattleGridManager();
            entities = new List<Entity>();
        }

        public void OnExit()
        {
        }

        public void EnterBattle()
        {
            Game.Event.Publish(new UIPanelEventArgs() { isShow = true, panelDefine = PanelDefine.BattleLoadingPanel });

            Game.GetModule<SceneManager>().EnterBattleScene();

            GridManager.InitGrid(10, 10);

            Game.GetModule<CameraManager>().SetState(CameraState.Battle);
            Game.Event.Publish(new SwitchInputModuleEventArgs() { moduleType = InputModuleType.Battle });
            Game.Event.Publish(new EnterBattleEventArgs() { isEnter = true });

            CreateBattleUnit();

            FlowManager.StartUp();

            isBattling = true;
        }

        public void ExitBattle()
        {
            isBattling = false;

            Game.GetModule<SceneManager>().ExitBattleScene();
            Game.Event.Publish(new EnterBattleEventArgs() { isEnter = false });
            Game.Event.Publish(new SwitchInputModuleEventArgs() { moduleType = InputModuleType.Character });
            Game.GetModule<CameraManager>().SetState(CameraState.Follow);

            foreach (var item in entities)
            {
                Game.GetModule<EntityManager>().DestroyEntity(item.Eid);
            }
            entities.Clear();
        }

        public void FixedUpdate()
        {
            if (isBattling)
            {
                FlowManager.Tick();
            }
        }

        private void CreateBattleUnit()
        {
            int count = 2;
            for (int i = 0; i < count; i++)
            {
                Entity entity = Game.GetModule<EntityManager>().CreateEntity<BattleUnitComponent, ActorComponent, BattleTransformComponent, BattleMotorComponent, AttributeComponent, BattleStateComponent>();
                var buc = entity.GetComponent<BattleUnitComponent>();
                buc.BattleId = i + 1;

                var ac = entity.GetComponent<ActorComponent>();
                ac.ActorType = ActorType.Battle;
                ac.ActorId = Utility.Math.Random(1001, 1003);
                ac.RefreshActor();
                ac.SetKinematic(true);
                ac.SyncRotation();

                var btf = entity.GetComponent<BattleTransformComponent>();
                btf.SetCoordPosition(4 + i, 5 + i);

                //GridManager.DrawTiles(4, 5, 3, TileState.Blue);

                var attrComponet = entity.GetComponent<AttributeComponent>();
                attrComponet.SetAttributeValue(AttributeDefine.HP_1, 100);
                attrComponet.SetAttributeValue(AttributeDefine.SP_1, 100);
                attrComponet.SetAttributeValue(AttributeDefine.SP_RATE, 10);
                attrComponet.AddAttributeEffect((int)AttributeDefine.SP_RATE + 10000, 5000);

                entities.Add(entity);

                FlowManager.AddUnit(entity);
            }
        }
    }
}