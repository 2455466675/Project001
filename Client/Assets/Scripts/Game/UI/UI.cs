using System.Reflection;
using GameFramework.Core;
using UnityEngine;

namespace GameFramework.UI 
{
    public static class UI
    {
        public static void Init()
        {
            Game.AddModule<UIManager>();

            AssemblyManager.AddAssembly(Assembly.GetExecutingAssembly());
        }
    }

    public struct UIEventTestArg : IGameEventArgs
    {
        public int id;
    }

    [GameEvent]
    public class UIEventTest : GameEventBase<UIEventTestArg>
    {
        public override void Invoke(UIEventTestArg arg)
        {
            Debug.Log("UIEventTest Invoke" + arg.id);
        }
    }
}