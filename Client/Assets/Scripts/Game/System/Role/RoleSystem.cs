namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleSystem
    {
        public ActorContainer Container { get; private set; }

        private Character character;

        public void Init(GameInitConfig config)
        {
            var go = Game.Resource.LoadAndInstantiate(config.ActorContainerPath, Game.Root.transform);
            Container = go.GetComponent<ActorContainer>();

            Character character = new Character();
            character.Init(810001);
            this.character = character;
        }

        public void Move(float x, float y) 
        {
            character.Move(x, y);
        }

        public void Run(bool isRunning)
        {
            character.Run(isRunning);
        }
    }
}
