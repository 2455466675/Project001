using System;
using Cysharp.Threading.Tasks;
using GameFramework.Logic;

namespace GameFramework.View.Action
{
    [Serializable]
    public abstract class BaseAction
    {
        public abstract UniTask Invoke(IActor actor);
    }
}
