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
    public class BattleSystem : IGameplaySystem
    {
        public BattleFlowManager FlowManager { get; private set; }
        public BattleGridManager GridManager { get; private set; }

        private List<Entity> entities;

        public Entity Entity => entities[0];

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

            Entity entity = Game.GetModule<EntityManager>().CreateEntity<ActorComponent, BattleTransformComponent, BattleMotorComponent, AttributeComponent>();
            ActorComponent ac = entity.GetComponent<ActorComponent>();
            ac.ActorType = ActorType.Battle;
            ac.ActorId = 1001;
            ac.RefreshActor();
            ac.SetKinematic(true);

            var btf = entity.GetComponent<BattleTransformComponent>();
            btf.SetCoordPosition(4, 5);

            GridManager.DrawTiles(4, 5, 3, TileState.Blue);

            var bmc = entity.GetComponent<BattleMotorComponent>();
            bmc.StartUp();

            var attrComponet = entity.GetComponent<AttributeComponent>();
            attrComponet.SetAttributeValue(AttributeDefine.HP_1, 100);
            attrComponet.SetAttributeValue(AttributeDefine.SP_1, 100);
            attrComponet.SetAttributeValue(AttributeDefine.SP_RATE, 5000);

            entities.Add(entity);
        }

        public void ExitBattle()
        {
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
    }
}