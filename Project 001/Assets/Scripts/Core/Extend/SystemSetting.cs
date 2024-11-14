using UnityEngine;

/// <summary>
/// 
/// </summary>
[CreateAssetMenu(menuName = "MyMenu/Create SystemSetting")]
public class SystemSetting : ScriptableObject
{
    public static SystemSetting Instance;

    public string[] bones;

    public static void LoadInstEditor()
    {
        Instance = Resources.Load<SystemSetting>("Game/SystemSetting");
    }

    public static string[] Bones()
    {
        if (Instance == null) return null;
        return Instance.bones;
    }
}

