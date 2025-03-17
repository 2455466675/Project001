using UnityEngine;
using UnityEditor;

/// <summary>
/// 
/// </summary>
public class TemplateMenuScript
{
    [MenuItem("GameObject/MyUI/DataSet")]
    public static void CreateDataSet()
    {
        string path = "Assets/Editor/UITemplate/DataSet.prefab";
        Instantiate(path);
    }

    [MenuItem("GameObject/MyUI/List/LoopNavigationList")]
    public static void CreateLoopNavigationList()
    {
        string path = "Assets/Editor/UITemplate/LoopNavigationList.prefab";
        Instantiate(path);
    }

    [MenuItem("GameObject/MyUI/List/FixedNavigationList")]
    public static void CreateFixedNavigationList()
    {
        string path = "Assets/Editor/UITemplate/FixedNavigationList.prefab";
        Instantiate(path);
    }

    [MenuItem("GameObject/MyUI/List/StaticNavigationList")]
    public static void CreateStaticNavigationList()
    {
        string path = "Assets/Editor/UITemplate/StaticNavigationList.prefab";
        Instantiate(path);
    }

    [MenuItem("GameObject/MyUI/Text/ExtendText")]
    public static void CreateExtendText()
    {
        string path = "Assets/Editor/UITemplate/ExtendText.prefab";
        Instantiate(path);
    }

    [MenuItem("GameObject/MyUI/Image/ExtendImage")]
    public static void CreateExtendImage()
    {
        string path = "Assets/Editor/UITemplate/ExtendImage.prefab";
        Instantiate(path);
    }

    [MenuItem("GameObject/MyUI/Text/FormatTextView")]
    public static void CreateFormatTextView()
    {
        string path = "Assets/Editor/UITemplate/FormatTextView.prefab";
        Instantiate(path);
    }

    [MenuItem("GameObject/MyUI/Text/StaticTextView")]
    public static void CreateStaticTextView()
    {
        string path = "Assets/Editor/UITemplate/StaticTextView.prefab";
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

