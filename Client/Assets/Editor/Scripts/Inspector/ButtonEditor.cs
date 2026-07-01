#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Linq;

[CustomEditor(typeof(MonoBehaviour), true)]
public class ButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // 获取所有带有Button特性的方法
        var methods = target.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => m.GetCustomAttributes(typeof(ButtonAttribute), false).Length > 0);

        foreach (var method in methods)
        {
            var buttonAttribute = (ButtonAttribute)method.GetCustomAttributes(typeof(ButtonAttribute), false)[0];

            string buttonText = string.IsNullOrEmpty(buttonAttribute.ButtonText)
                ? ObjectNames.NicifyVariableName(method.Name)
                : buttonAttribute.ButtonText;

            GUILayout.Space(5);

            // 设置按钮高度
            var rect = GUILayoutUtility.GetRect(0, buttonAttribute.ButtonHeight);

            if (GUI.Button(rect, buttonText))
            {
                // 记录撤销操作
                Undo.RecordObject(target, $"Execute {method.Name}");

                try
                {
                    method.Invoke(target, null);
                    EditorUtility.SetDirty(target);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error executing method {method.Name}: {e.Message}");
                }
            }
        }
    }
}
#endif
