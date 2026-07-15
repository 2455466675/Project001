using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Logic
{
    public class PuppetBone : MonoBehaviour
    {
        private class PuppetWidget
        {
            public string assetPath;
            public GameObject asset;
            public float duration;
            public float timer;

            public void Release()
            {
                Game.Assets.ReleaseAsset(asset);
                asset = null;
            }
        }

        [SerializeField]
        private Transform[] bones;

        private Dictionary<string , PuppetWidget> widgets = new Dictionary<string, PuppetWidget>();
        private List<PuppetWidget> widgetsList = new List<PuppetWidget>();

        public void AddWidget(PuppetWidgetArgs args)
        {
            if (!string.IsNullOrEmpty(args.key))
            {
                if (widgets.ContainsKey(args.key))
                {
                    return;
                }
                var widget = CreateWidget(args);
                if (widget == null)
                {
                    return;
                }
                widgets.Add(args.key, widget);
            }
            else
            {
                if (args.duration < 0f)
                {
                    return;
                }
                var widget = CreateWidget(args);
                if (widget == null)
                {
                    return;
                }
                widgetsList.Add(widget);
            }
        }

        public void RemoveWidget(string key)
        {
            if (!widgets.ContainsKey(key))
            {
                return;
            }
            widgets[key].Release();
            widgets.Remove(key);
        }

        private void Update()
        {
            int count = widgetsList.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                var widget = widgetsList[i];
                if (widget.timer >= widget.duration)
                {
                    widget.Release();
                    widgetsList.RemoveAt(i);
                }
                else
                {
                    widget.timer += Time.deltaTime;
                }
            }
        }

        public Transform GetBone(string boneName)
        {
            if (bones == null || bones.Length == 0)
            {
                return null;
            }

            foreach (var item in bones)
            {
                if (item.gameObject.name == boneName)
                {
                    return item;
                }
            }

            return bones[0];
        }

        private PuppetWidget CreateWidget(PuppetWidgetArgs args)
        {
            var assetPath = args.assetPath;
            if (string.IsNullOrEmpty(assetPath))
            {
                return null;
            }

            var boneName = args.boneName;
            var bone = GetBone(boneName);
            if (bone == null)
            {
                bone = this.transform;
            }
            var asset = Game.Assets.Instantiate(assetPath, bone);
            if (asset == null)
            {
                return null;
            }
            asset.transform.localRotation = Quaternion.Euler(args.rotation);
            asset.transform.localScale = args.scale;

            PuppetWidget widget = new PuppetWidget();
            widget.assetPath = assetPath;
            widget.asset = asset;
            widget.duration = args.duration;
            widget.timer = 0f;
            return widget;
        }
    }
}
