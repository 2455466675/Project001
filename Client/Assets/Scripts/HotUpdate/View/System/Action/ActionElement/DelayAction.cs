using UnityEngine;
using Cysharp.Threading.Tasks;
using GameFramework.Logic;

namespace GameFramework.View.Action
{
    public class DelayAction : BaseAction
    {
        [Min(0f)]
        public float delay;
        public override async UniTask Invoke(IActor actor)
        {
            await UniTask.WaitForSeconds(delay);
        }
    }
}
