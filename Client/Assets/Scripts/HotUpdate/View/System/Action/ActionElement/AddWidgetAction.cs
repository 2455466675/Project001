using Cysharp.Threading.Tasks;
using GameFramework.Logic;
using UnityEngine;

namespace GameFramework.View.Action
{
    public class AddWidgetAction : BaseAction
    {
        public string assetPath;
        public string boneName;
        public float duration;
        public Vector3 scale = Vector3.one;
        public Vector3 rotation = Vector3.zero;

        public override async UniTask Invoke(IActor actor)
        {
            if (actor == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(assetPath))
            {
                return;
            }

            PuppetWidgetArgs args = new PuppetWidgetArgs();
            args.assetPath = assetPath;
            args.boneName = boneName;
            args.duration = duration;
            args.scale = scale;
            args.rotation = rotation;
            actor.AddWidget(args);

            await UniTask.CompletedTask;
        }
    }
}
