using GameFramework;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class GenerateSpriteMap
{
    [MenuItem("Tools/MyTools/Generate Sprite Map")]
    public static void Generate() 
    {
        SpritePathListWrapper map = new SpritePathListWrapper();

        Dictionary<string, SpritePathData> data = new Dictionary<string, SpritePathData>();

        List<SpriteAtlas> spriteAtlas = LoadAllAssetsInFolder<SpriteAtlas>("t:SpriteAtlas", "Assets/Bundles/ArtResources");
        Debug.Log("spriteAtlas.Count:" + spriteAtlas.Count);

        foreach (var atlas in spriteAtlas)
        {
            Sprite[] temp = new Sprite[atlas.spriteCount];
            atlas.GetSprites(temp);

            string atlasPath = System.IO.Path.ChangeExtension(AssetDatabase.GetAssetPath(atlas), null);

            foreach (var s in temp)
            {
                string name = s.name;
                if (name.EndsWith("(Clone)"))
                {
                    name = name.Replace("(Clone)", string.Empty);
                }

                SpritePathData item = new SpritePathData();
                item.spriteName = name;
                item.isMultiple = false;
                item.assetPath = atlasPath;
                
                if (data.ContainsKey(name))
                {
                    Debug.LogError($"命名重复:{name}");
                    continue;
                }
                else
                {
                    data.Add(name, item);
                }
            }
        }

        List<Sprite> sprites = LoadAllAssetsInFolder<Sprite>("t:Sprite", "Assets/Bundles/ArtResources");
        Debug.Log("sprites.Count:" + sprites.Count);

        foreach (var s in sprites)
        {
            string name = s.name;
            if (name.EndsWith("(Clone)"))
            {
                name = name.Replace("(Clone)", string.Empty);
            }

            if (data.ContainsKey(name))
            {
                Debug.LogError($"命名重复:{name}");
                continue;
            }

            SpritePathData item = new SpritePathData();
            item.spriteName = name;
            item.isMultiple = true;            
            item.assetPath = System.IO.Path.ChangeExtension(AssetDatabase.GetAssetPath(s), null);

            data.Add(name, item);
        }

        map.items = new List<SpritePathData>(data.Values);

        string bundlePath = "Bundles/Common/SpritePathMap.json";
        string json = LITJson.JsonMapper.ToJson(map);
        string path = Path.Combine(Application.dataPath, bundlePath);

        File.WriteAllText(path, json);

        Debug.Log("Generate success : " + path);
        AssetDatabase.Refresh();
    }


    private static List<T> LoadAllAssetsInFolder<T>(string t, string folderPath) where T : UnityEngine.Object
    {
        List<T> atlasList = new List<T>();

        string[] guids = AssetDatabase.FindAssets(t, new[] { folderPath });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            T atlas = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if (atlas != null)
            {
                atlasList.Add(atlas);
            }
        }

        return atlasList;
    }

    //private void Test()
    //{
    //    Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);
    //    tex.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin,
    //        (int)sprite.rect.width, (int)sprite.rect.height));
    //    tex.Apply();

    //    // 写入成PNG文件
    //    System.IO.File.WriteAllBytes(outPath + "/" + sprite.name + ".png", tex.EncodeToPNG());
    //}
}