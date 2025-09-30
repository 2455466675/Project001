using System.Reflection;
using GameFramework.Core;
using UnityEngine;

namespace GameFramework.UI 
{
    public static class UI
    {
        public static void Init()
        {
            Game.AddModule<InputController>();
            Game.AddModule<UIManager>();

            AssemblyManager.AddAssembly(Assembly.GetExecutingAssembly());
        }
    }
}