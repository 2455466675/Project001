namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleSystem
    {
        private ActorContainer container;

        public void Init(GameInitConfig config)
        {
            var go = Game.Resource.LoadAndInstantiate(config.ActorContainerPath, Game.Root.transform);
            container = go.GetComponent<ActorContainer>();

            //CharacterEntity character = CreateChild<CharacterEntity>();
            //character.AddComponent<ActorComponent>();
        }
    }
}
