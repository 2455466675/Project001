using System.Reflection;
using GameFramework.Core;

namespace GameFramework.Gameplay 
{
    public static class Gameplay
    {
        public static void Init()
        {
            AssemblyManager.AddAssembly(Assembly.GetExecutingAssembly());
        }
    }
}