using GameFramework.Core;
using GameFramework.Featrue;
using System.Collections.Generic;

namespace GameFramework.Gameplay
{
    [Gameplay]
    public class PartySystem : IGameplaySystem, IGameSerializeable
    {
        private Entity character;

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

            character = entity1;
        }

        public void OnExit()
        {

        }

        public void OnSaveGame(ISaveWriter writer)
        {

        }

        public void OnLoadGame(ISaveReader reader)
        {
            var ac = character.GetComponent<ActorComponent>();
            ac.ActorId = 1001;
            ac.ActorType = ActorType.Party;
            ac.RefreshActor();
            ac.Position = new UnityEngine.Vector3(0, 0, 0);

            Game.GetModule<CameraManager>().SetTarget(ac.Transform);

            var mc = character.GetComponent<MotorComponent>();
            mc.SetFollowTarget(null);
            mc.StartUp();

            for (int i = 1; i < characters.Count; i++)
            {
                var entity = characters[i];
                var ac2 = entity.GetComponent<ActorComponent>();
                ac2.ActorId = Utility.Math.Random(1001, 1003);
                ac2.ActorType = ActorType.Party;
                ac2.RefreshActor();
                ac2.Position = new UnityEngine.Vector3(0, 0, 0);

                var mc2 = entity.GetComponent<MotorComponent>();
                mc2.SetFollowTarget(characters[i - 1].GetComponent<MotorComponent>());
                mc2.StartUp();
            }
        }

        public Entity GetLeader()
        {
            return character; 
        }

        private Entity CreateCharacter()
        {
            Entity entity = Game.GetModule<EntityManager>().CreateEntity<ActorComponent, MotorComponent>();
            return entity;
        }
    }
}