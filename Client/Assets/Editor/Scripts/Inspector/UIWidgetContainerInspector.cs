using GameFramework.UI;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIPanel))]
public class UIPanelInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UIPanel container = (UIPanel)target;
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Init", GUILayout.Width(100), GUILayout.Height(26)))
        {
            MethodInfo method = typeof(UIWidgetContainer).GetMethod("InitEditor", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            method?.Invoke(container, null);
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
    }
}

[CustomEditor(typeof(NavigationItemView))]
public class NavigationItemViewInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NavigationItemView container = (NavigationItemView)target;
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Init", GUILayout.Width(100), GUILayout.Height(26)))
        {
            MethodInfo method = typeof(UIWidgetContainer).GetMethod("InitEditor", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            method?.Invoke(container, null);
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
    }
}
