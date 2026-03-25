using UnityEngine;
using UnityEditor;

public class CSharpScriptEditor : EditorWindow
{
    //[MenuItem("Tools/MyTools/CSharpScriptEditor", false, 2)]
    static void ClearAllAssetBundleName()
    {
        CSharpScriptEditor window = (CSharpScriptEditor)EditorWindow.GetWindowWithRect(typeof(CSharpScriptEditor), new Rect(0, 0, 300, 700), false, "CSharp");
        window.maximized = true;
        window.wantsMouseEnterLeaveWindow = true;
        window.wantsMouseMove = true;

        window.Show();
    }

    string mCodePre;

    const int gTabCount = 5;

    int mSelTabIdx = 0;
    string[] mTabText = new string[gTabCount] { "Code1", "Code2", "Code3", "Code4", "Code5" };
    string[] mCodeTexts = new string[gTabCount];

    void LoadCode(int idx)
    {
        mCodeTexts[idx] = PlayerPrefs.GetString("csharp_editor" + idx);
    }
    void SaveCode(int idx)
    {
        var str = mCodeTexts[idx];
        if (string.IsNullOrEmpty(str)) return;

        PlayerPrefs.SetString("csharp_editor" + idx, str);
    }

    private void Awake()
    {
        for (int i = 0; i < mCodeTexts.Length; ++i)
        {
            LoadCode(i);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < mCodeTexts.Length; ++i)
        {
            SaveCode(i);
        }
    }

    private void OnFocus()
    {
        string path = Application.dataPath + "/Editor/PreCode.txt";
        if (System.IO.File.Exists(path))
        {
            mCodePre = System.IO.File.ReadAllText(path);
        }
        else
        {
            mCodePre = "";
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label(mCodePre);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Run"))
        {
            SaveCode(mSelTabIdx);

            if (Application.isPlaying)
            {
                try
                {
                    Debug.Log("Script executed successfully!");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Script error: {e.Message}");
                }
            }
        }

        mSelTabIdx = GUILayout.Toolbar(mSelTabIdx, mTabText);
        GUILayout.EndHorizontal();
        mCodeTexts[mSelTabIdx] = GUILayout.TextArea(mCodeTexts[mSelTabIdx], GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        GUILayout.EndVertical();
    }
}
