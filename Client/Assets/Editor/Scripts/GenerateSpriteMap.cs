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

        List<SpriteAtlas> spriteAtlas = LoadAllAssetsInFolder<SpriteAtlas>("t:SpriteAtlas", SpritePathData.RootPath);
        Debug.Log("spriteAtlas.Count:" + spriteAtlas.Count);

        foreach (var atlas in spriteAtlas)
        {
            Sprite[] temp = new Sprite[atlas.spriteCount];
            atlas.GetSprites(temp);

            string atlasPath = ToRelativePath(System.IO.Path.ChangeExtension(AssetDatabase.GetAssetPath(atlas), null));

            foreach (var s in temp)
            {
                string name = s.name;
                if (name.EndsWith("(Clone)"))
                {
                    name = name.Replace("(Clone)", string.Empty);
                }

                // spriteName 是运行时映射的唯一键，重名会导致取到非预期资源，因此冲突时保留先注册者并报出两个来源便于排查。
                if (data.ContainsKey(name))
                {
                    Debug.LogError($"精灵名重复，已跳过图集精灵: {name} (已存在: {data[name].assetPath}, 冲突图集: {atlasPath})");
                    continue;
                }

                SpritePathData item = new SpritePathData();
                item.spriteName = name;
                item.isMultiple = false;
                item.assetPath = atlasPath;

                data.Add(name, item);
            }
        }

        List<Sprite> sprites = LoadAllAssetsInFolder<Sprite>("t:Sprite", SpritePathData.RootPath);
        Debug.Log("sprites.Count:" + sprites.Count);

        foreach (var s in sprites)
        {
            string name = s.name;
            if (name.EndsWith("(Clone)"))
            {
                name = name.Replace("(Clone)", string.Empty);
            }

            string assetPath = ToRelativePath(System.IO.Path.ChangeExtension(AssetDatabase.GetAssetPath(s), null));

            // 同上：散图与图集精灵可能同名，冲突时保留先注册者（图集先扫描，故图集优先）。
            if (data.ContainsKey(name))
            {
                Debug.LogError($"精灵名重复，已跳过散图: {name} (已存在: {data[name].assetPath}, 冲突散图: {assetPath})");
                continue;
            }

            SpritePathData item = new SpritePathData();
            item.spriteName = name;
            item.isMultiple = true;
            item.assetPath = assetPath;

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


    private static string ToRelativePath(string assetPath)
    {
        string prefix = SpritePathData.RootPath + "/";
        if (assetPath.StartsWith(prefix))
        {
            return assetPath.Substring(prefix.Length);
        }
        return assetPath;
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
}
