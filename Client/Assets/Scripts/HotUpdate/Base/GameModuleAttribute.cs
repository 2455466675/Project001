namespace GameFramework 
{
    public class GameModuleAttribute : GameAttribute
    {
        public GameModulePriority Priority { get; private set; }

        public GameModuleAttribute(GameModulePriority priority)
        {
            Priority = priority;
        }
    }
}