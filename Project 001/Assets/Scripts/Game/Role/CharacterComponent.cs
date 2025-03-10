using EC;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class CharacterComponent : EC.Component, IAwake
    {
        public void Awake()
        {
            MyEntity.AddComponent<QueueableComponent>();
            MyEntity.AddComponent<ActorComponent>();
            MyEntity.AddComponent<MotorComponent>();
        }

        public void Init(int id) 
        {
            MyEntity.GetComponent<ActorComponent>().Init(id);
        }
    }
}
