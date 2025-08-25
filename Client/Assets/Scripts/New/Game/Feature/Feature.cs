using System.Reflection;
using GameFramework.Core;

namespace GameFramework.Featrue 
{
    public static class Feature
    {
        public static void Init()
        {
            Game.AddModule<EntityManager>();

            AssemblyManager.AddAssembly(Assembly.GetExecutingAssembly());
        }
    }
}