using System.Reflection;
using GameFramework.Core;

namespace GameFramework.Featrue 
{
    public static class Feature
    {
        public static void Init()
        {
            Game.AddModule<EntityFactory>();

            AssemblyManager.AddAssembly(Assembly.GetExecutingAssembly());
        }
    }
}