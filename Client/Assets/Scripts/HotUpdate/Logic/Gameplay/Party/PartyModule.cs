using ECS;
using GameFramework.Core;
using System.Collections.Generic;

namespace GameFramework.Logic
{
    [Gameplay]
    public class PartyModule : IGameplay, IGameSavable
    {
        public Entity Leader => leader;
        private Entity leader;
        private List<Entity> characters;

        void IGameplay.OnInit()
        {
            Entity entity1 = CreateCharacter();
            Entity entity2 = CreateCharacter();
            Entity entity3 = CreateCharacter();
            Entity entity4 = CreateCharacter();

            characters = new List<Entity>();
            characters.Add(entity1);
            characters.Add(entity2);
            characters.Add(entity3);
            characters.Add(entity4);

            leader = entity1;
        }

        void IGameplay.OnExit()
        {

        }

        void IGameSavable.OnSaveGame(IWriter writer)
        {
            
        }

        void IGameSavable.OnLoadGame(IReader reader)
        {
            MDebug.Log("OnLoadGame");
            var ac = leader.GetComponent<ActorComponent>();
            ac.SetActorType(ActorType.Party);
            ac.SetActorId(1001);
            ac.SetPosition(1f, 0f, 1f);
            ac.RefreshActor();
            var mc = leader.GetComponent<MotorComponent>();
            mc.SetFollowTarget(0);

            for (int i = 1; i < characters.Count; i++)
            {
                var entity = characters[i];
                var ac2 = entity.GetComponent<ActorComponent>();
                ac2.SetActorId(Utility.Util.Math.Random(1001, 1003));
                ac2.SetActorType(ActorType.Party);
                ac2.SetPosition(1f, 0f, 1f);
                ac2.RefreshActor();

                var mc2 = entity.GetComponent<MotorComponent>();
                mc2.SetFollowTarget(characters[i - 1].Eid);
            }
        }
        
        public void StartUp()
        {
            for (int i = 0; i < characters.Count; i++)
            {
                var entity = characters[i];
                var ac = entity.GetComponent<ActorComponent>();
                ac.SetVisable(true);

                var mc = entity.GetComponent<MotorComponent>();                
                mc.StartUp();
            }
        }

        public void ShutDown()
        {
            for (int i = 0; i < characters.Count; i++)
            {
                var entity = characters[i];
                var ac = entity.GetComponent<ActorComponent>();
                ac.SetVisable(false);

                var mc = entity.GetComponent<MotorComponent>();
                mc.ShutDown();
            }
        }

        public void OnInput(InputContext context)
        {
            Leader?.GetComponent<MotorComponent>().OnInputAction(context);
        }

        private Entity CreateCharacter()
        {
            Entity entity = Game.GetSystem<GameEntityFactory>().CreateEntity<MemberComponent, ActorComponent, MotorComponent, MotorAnimatorComponent>();
            return entity;
        }
    }
}