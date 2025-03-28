using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.U2D;

namespace Game
{
    [Serializable]
    public class SpriteItem
    {
        public string name;
        public string assetPath;
        public bool isAtlasItem;
        public string atlasPath;
    }

    [Serializable]
    public class SpriteMap
    {
        public Dictionary<string, SpriteItem> data;
    }

    public class SpriteManager
    {
        private Dictionary<string, SpriteItem> data;
        private Dictionary<string, SpriteAtlas> spriteAtlas;

        public void Init(GameInitConfig initCfg) 
        {
            TextAsset asset = Resources.Load<TextAsset>(initCfg.SpriteMap);
            if (asset != null) 
            {
                SpriteMap map = JsonConvert.DeserializeObject<SpriteMap>(asset.text);
                if (map != null) 
                {
                    data = map.data;                            
                }
            }

            spriteAtlas = new Dictionary<string, SpriteAtlas>();
        }

        public Sprite GetSprite(string name) 
        {
            if (data == null) 
            {
                return null;
            }

            if (!data.ContainsKey(name)) 
            {
                return null;
            }

            SpriteItem item = data[name];
            if (item.isAtlasItem) 
            {
                string atlasPath = item.atlasPath;
                SpriteAtlas atlas;
                if (spriteAtlas.ContainsKey(atlasPath))
                {
                    atlas = spriteAtlas[atlasPath];
                }
                else
                {
                    atlas = GameWorld.Root.GetComponent<ResourceComponent>().LoadAsset<SpriteAtlas>(atlasPath);
                    if (atlas != null) 
                    {
                        spriteAtlas[atlasPath] = atlas;                    
                    }
                }
                
                if(atlas == null) 
                {
                    return null;
                }
                else
                {
                    return atlas.GetSprite(name);
                }
            }
            else
            {
                return GameWorld.Root.GetComponent<ResourceComponent>().LoadAsset<Sprite>(item.assetPath);
            }
        }
    }
}