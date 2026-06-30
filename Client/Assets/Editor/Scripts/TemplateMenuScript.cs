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
        string path = "Assets/Bundles/UI/Prefabs/UITemplate/FluidNavigationList.prefab";
        Instantiate(path);
    }

    [MenuItem("GameObject/MyUI/List/FixedNavigationList")]
    public static void CreateFixedNavigationList()
    {
        string path = "Assets/Bundles/UI/Prefabs/UITemplate/FixedNavigationList.prefab";
        Instantiate(path);
    }

    [MenuItem("GameObject/MyUI/List/NavigationListBar")]
    public static void CreateNavigationListBar()
    {
        string path = "Assets/Bundles/UI/Prefabs/UITemplate/NavigationListBar.prefab";
        Instantiate(path);
    }

    [MenuItem("GameObject/MyUI/Text/TextView")]
    public static void CreateTextView()
    {
        string path = "Assets/Bundles/UI/Prefabs/UITemplate/TextView.prefab";
        Instantiate(path);
    }

    [MenuItem("GameObject/MyUI/Image/ImageView")]
    public static void CreateImageView()
    {
        string path = "Assets/Bundles/UI/Prefabs/UITemplate/ImageView.prefab";
        Instantiate(path);
    }

    [MenuItem("GameObject/MyUI/Text/StaticTextView")]
    public static void CreateStaticTextView()
    {
        string path = "Assets/Bundles/UI/Prefabs/UITemplate/StaticTextView.prefab";
        Instantiate(path);
    }

    [MenuItem("GameObject/MyUI/SliderView")]
    public static void CreateSliderView()
    {
        string path = "Assets/Bundles/UI/Prefabs/UITemplate/SliderView.prefab";
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

