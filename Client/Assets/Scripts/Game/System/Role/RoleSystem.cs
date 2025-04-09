namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleSystem
    {
        public ActorContainer Container { get; private set; }

        public void Init(GameInitConfig config)
        {
            var go = Game.Resource.LoadAndInstantiate(config.ActorContainerPath, Game.Root.transform);
            Container = go.GetComponent<ActorContainer>();

            Character character = new Character();
            character.Init(810001);
        }
    }
}
