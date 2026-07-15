using UnityEngine;
using Cysharp.Threading.Tasks;
using GameFramework.Logic;

namespace GameFramework.View.Action
{
    public class AnimationAction : BaseAction
    {
        public string animationName;
        public bool useAnimTime;
        [Min(0f)]
        public float awaitTime;
        public override async UniTask Invoke(IActor actor)
        {
            if (actor == null)
            {
                return;
            }
            float animationTime = actor.PlayAnimation(animationName);
            float t = useAnimTime ? animationTime : awaitTime;
            await UniTask.WaitForSeconds(t);
        }
    }
}
