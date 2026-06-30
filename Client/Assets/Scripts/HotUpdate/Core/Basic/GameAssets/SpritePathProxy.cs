using Cysharp.Threading.Tasks;
using LITJson;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
{
    internal class SpritePathProxy
    {
        private Dictionary<string, SpritePathData> map;
        
        public async UniTask Init()
        {
            string path = "Assets/Bundles/Common/SpritePathMap.json";
            TextAsset textAsset = await Game.Assets.LoadAssetAsync<TextAsset>(path);
            if (textAsset != null)
            {
                SpritePathListWrapper wrapper = JsonMapper.ToObject<SpritePathListWrapper>(textAsset.text);
                List<SpritePathData> items = (wrapper != null && wrapper.items != null) ? wrapper.items : new List<SpritePathData>();

                map = new Dictionary<string, SpritePathData>(items.Count);
                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    map[item.spriteName] = item;
                }
                Game.Assets.ReleaseAsset(textAsset);
            }
            else
            {
                MDebug.Error($"SpritePathProxy Init Fail ! : {path}");
                map = new Dictionary<string, SpritePathData>();
            }
        }

        public string GetAssetPath(string spriteName, out bool isMultiple)
        {
            if (map.TryGetValue(spriteName, out var data))
            {
                isMultiple = data.isMultiple;
                return data.assetPath;
            }
            else
            {
                isMultiple = false;
                return "";
            }
        }
    }
}