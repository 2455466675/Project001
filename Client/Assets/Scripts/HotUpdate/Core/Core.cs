using System.Reflection;

namespace GameFramework.Core 
{
    public static class Core
    {
        public static void Init() 
        {
            Game.AddModule<ResourceManager>();
            Game.AddModule<ConfigManager>();
            Game.AddModule<AssemblyManager>();
            Game.AddModule<EventManager>();
            Game.AddModule<InputManager>();

            AssemblyManager.AddAssembly(Assembly.GetExecutingAssembly());
        }
    }
}