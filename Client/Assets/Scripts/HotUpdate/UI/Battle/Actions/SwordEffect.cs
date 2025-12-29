using GameFramework.Core;
using GameFramework.Gameplay;
using UnityEngine;

namespace GameFramework.UI
{
    public class SwordEffect : ActionCommand
    {
        public string assetPath;

        public Vector3 offest = Vector3.zero;
        public Vector3 scale = Vector3.one;
        public Vector3 rotation = Vector3.zero;

        private GameObject instance;

        protected override void OnExecute()
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return;
            }

            if (actionData == null || actionData.actor == null)
            {
                return;
            }

            IActor actor = actionData.actor;

            var canvas = GameRoot.GetNode<BattleNode>().EffectCanvas;
            instance = Game.GetModule<AssetsManager>().LoadAndInstantiate(assetPath, canvas);    
            if (instance != null)
            {
                Vector3 pos = actor.GetBone("center").position;
                instance.transform.position = pos + offest;
                instance.transform.localScale = scale;

                int dir = actionData.direction;
 
                var r = instance.transform.rotation;
                r *= Quaternion.Euler(rotation.x, rotation.y + 90 * (dir - 1), rotation.z);
                instance.transform.rotation = r;
            }
        }

        protected override void OnComplete()
        {
            if (instance != null)
            {
                GoHelper.Destroy(instance);
                instance = null;
            }
        }
    }
}
