using UnityEngine;
using UnityEditor;

/// <summary>
/// 
/// </summary>
public class TemplateMenuScript
{
    [MenuItem("GameObject/MyUI/LoopNavigationList")]
    public static void CreateLoopNavigationList()
    {
        string path = "Assets/Editor/UITemplate/LoopNavigationList.prefab";
        GameObject inst = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(path));
        inst.name = inst.name.Replace("(Clone)", string.Empty);
        GameObjectUtility.SetParentAndAlign(inst, Selection.activeGameObject);
    }

    [MenuItem("GameObject/MyUI/FixedNavigationList")]
    public static void CreateFixedNavigationList()
    {
        string path = "Assets/Editor/UITemplate/FixedNavigationList.prefab";
        GameObject inst = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(path));
        inst.name = inst.name.Replace("(Clone)", string.Empty);
        GameObjectUtility.SetParentAndAlign(inst, Selection.activeGameObject);
    }
}

