using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class EditorInitialization
{
    // 静态构造函数将在编辑器加载时自动调用
    static EditorInitialization()
    {
        Debug.Log("Startup!");
        SystemSetting.LoadInstEditor();
    }
}
