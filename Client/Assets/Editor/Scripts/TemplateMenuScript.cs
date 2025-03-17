using UnityEngine;
using UnityEditor;

/// <summary>
/// 
/// </summary>
public class TemplateMenuScript
{

    [MenuItem("GameObject/MyUI/List/FluidNavigationList")]
    public static void CreateLoopNavigationList()
    {
        string path = "Assets/Editor/UITemplate/FluidNavigationList.prefab";
        Instantiate(path);
    }

    [MenuItem("GameObject/MyUI/List/FixedNavigationList")]
    public static void CreateFixedNavigationList()
    {
        string path = "Assets/Editor/UITemplate/FixedNavigationList.prefab";
        Instantiate(path);
    }

    private static void Instantiate(string path)
    {
        GameObject inst = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(path));
        inst.name = inst.name.Replace("(Clone)", string.Empty);
        GameObjectUtility.SetParentAndAlign(inst, Selection.activeGameObject);
        Selection.activeGameObject = inst;
    }
}

