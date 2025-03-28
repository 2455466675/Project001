using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using Game;


public class GenerateSpriteMap : MonoBehaviour
{
    [MenuItem("Tools/MyTools/GenerateSpriteMap")]
    public static void Generate() 
    {
        SpriteMap map = new SpriteMap();

        Dictionary<string, SpriteItem> data = new Dictionary<string, SpriteItem>();

        List<SpriteAtlas> spriteAtlas = LoadAllAssetsInFolder<SpriteAtlas>("t:SpriteAtlas", "Assets/Bundles/ArtRes");
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

                SpriteItem item = new SpriteItem();
                item.name = name;
                item.isAtlasItem = true;
                item.atlasPath = atlasPath;
                item.assetPath = string.Empty;

                data[name] = item;
            }
        }

        List<Sprite> sprites = LoadAllAssetsInFolder<Sprite>("t:Sprite", "Assets/Bundles/ArtRes");
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
                continue;
            }

            SpriteItem item = new SpriteItem();
            item.name = name;
            item.isAtlasItem = false;
            item.atlasPath = string.Empty;
            item.assetPath = System.IO.Path.ChangeExtension(AssetDatabase.GetAssetPath(s), null);

            data[name] = item;

        }

        map.data = data;

        string json = JsonConvert.SerializeObject(map);

        string path = Path.Combine(Application.dataPath, "Resources/Game/sprite_map.json");

        File.WriteAllText(path, json);

        Debug.Log("Generate success : " + "Resources/Game/sprite_map.json");
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