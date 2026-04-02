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

        public void OnInit()
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

        public void OnExit()
        {

        }

        public void OnSaveGame(IWriter writer)
        {
            
        }

        public void OnLoadGame(IReader reader)
        {
            MDebug.Log("OnLoadGame");
            var ac = leader.GetComponent<ActorComponent>();
            ac.SetActorType(ActorType.Party);
            ac.SetActorId(1001);
            ac.RefreshActor();
            ac.Position = new UnityEngine.Vector3(0, 0, 0);

            Game.GetSystem<GameCameraController>().SetFollowTarget(ac.Transform);

            var mc = leader.GetComponent<MotorComponent>();
            mc.SetFollowTarget(null);
            mc.StartUp();

            for (int i = 1; i < characters.Count; i++)
            {
                var entity = characters[i];
                var ac2 = entity.GetComponent<ActorComponent>();
                ac2.SetActorId(Utility.Util.Math.Random(1001, 1003));
                ac2.SetActorType(ActorType.Party);
                ac2.RefreshActor();
                ac2.Position = new UnityEngine.Vector3(0, 0, 0);

                var mc2 = entity.GetComponent<MotorComponent>();
                mc2.SetFollowTarget(characters[i - 1].GetComponent<MotorComponent>());
                mc2.StartUp();
            }
        }

        public void OnInput(InputContext context)
        {
            Leader?.GetComponent<MotorComponent>().OnInputAction(context);
        }

        private Entity CreateCharacter()
        {
            Entity entity = Game.GetSystem<GameEntityFactory>().CreateEntity<ActorComponent, MotorComponent>();
            return entity;
        }
    }
}