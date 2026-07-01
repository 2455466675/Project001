using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using GameFramework;

public class GamePriorityEditorWindow : EditorWindow
{
    private List<ClassPriorityData> classDataList = new List<ClassPriorityData>();
    private Vector2 scrollPosition;
    private string attributeName = "";
    private string jsonFilePath = "Bundles/Common/{0}.json";

    private int selectedIndex = 0;
    private string[] options = new string[] { "GameSystemAttribute", "GameplayAttribute", };

    [MenuItem("Tools/MyTools/Game Priority Editor")]
    public static void ShowWindow()
    {
        GetWindow<GamePriorityEditorWindow>("Game Priority Editor");
    }

    private void OnEnable()
    {
        selectedIndex = 0;
        attributeName = options[selectedIndex];
        LoadClassesWithAttribute();
    }

    private void LoadClassesWithAttribute()
    {
        InitializeClassDataList();
        LoadPriorityFromJson();
    }

    private void InitializeClassDataList()
    {
        classDataList.Clear();

        // 查找Attribute类型
        Type attributeType = FindAttributeType(attributeName);

        if (attributeType == null)
        {
            Debug.LogWarning($"未找到Attribute类型: {attributeName}");
            return;
        }

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly assembly in assemblies)
        {
            try
            {
                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    // 使用找到的类型进行检查
                    if (Attribute.IsDefined(type, attributeType))
                    {
                        int priority = 0;
                        var existingData = classDataList.Find(d => d.typeFullName == type.FullName);
                        if (existingData != null)
                        {
                            priority = existingData.priority;
                        }
                        classDataList.Add(new ClassPriorityData() { typeFullName = type.FullName, priority = priority });
                    }
                }
            }
            catch (ReflectionTypeLoadException)
            {
                continue;
            }
        }
    }

    private void LoadPriorityFromJson()
    {
        string path = string.Format(this.jsonFilePath, attributeName);
        string fullPath = Path.Combine(Application.dataPath, path.Replace("Assets/", ""));
        fullPath = Path.GetFullPath(fullPath);
        if (File.Exists(fullPath))
        {
            try
            {
                string json = File.ReadAllText(fullPath);
                ClassPriorityListWrapper wrapper = LITJson.JsonMapper.ToObject<ClassPriorityListWrapper>(json);
                if (wrapper != null && wrapper.items != null)
                {
                    foreach (var data in wrapper.items)
                    {
                        var existingData = classDataList.Find(d => d.typeFullName == data.typeFullName);
                        if (existingData != null)
                        {
                            existingData.priority = data.priority;
                        }
                    }

                    classDataList = classDataList.OrderBy(c => c.priority).ToList();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"从JSON文件加载优先级失败: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning("配置文件不存在：" + fullPath);
        }
    }

    private Type FindAttributeType(string typeName)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly assembly in assemblies)
        {
            try
            {
                // 尝试完全限定名
                Type type = assembly.GetType(typeName);
                if (type != null && typeof(Attribute).IsAssignableFrom(type))
                    return type;

                // 尝试简单名称搜索
                type = assembly.GetTypes()
                    .FirstOrDefault(t => typeof(Attribute).IsAssignableFrom(t) &&
                                       (t.Name == typeName || t.FullName == typeName));
                if (type != null)
                    return type;
            }
            catch
            {
                continue;
            }
        }

        return null;
    }

    private void OnGUI()
    {
        GUILayout.Label("编辑优先级", EditorStyles.boldLabel);

        EditorGUILayout.Space(5);
        EditorGUI.BeginChangeCheck();
        selectedIndex = EditorGUILayout.Popup("选择选项", selectedIndex, options);
        if (EditorGUI.EndChangeCheck())
        {
            attributeName = options[selectedIndex];
            LoadClassesWithAttribute();
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Attribute类名:", GUILayout.Width(100));
        EditorGUILayout.LabelField(attributeName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("JSON文件路径:", GUILayout.Width(100));
        string path = string.Format(jsonFilePath, attributeName);
        EditorGUILayout.LabelField(path);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);

        if (classDataList.Count == 0)
        {
            EditorGUILayout.HelpBox($"未找到带有 '{attributeName}' 特性的类。", MessageType.Info);
        }
        else
        {
            EditorGUILayout.LabelField($"找到 {classDataList.Count} 个类", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < classDataList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(classDataList[i].typeFullName, GUILayout.Width(position.width - 150));
                classDataList[i].priority = EditorGUILayout.IntField(classDataList[i].priority, GUILayout.Width(100));
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("保存", GUILayout.Height(30)))
        {
            SaveToJson();
        }

        EditorGUILayout.Space(5);

        if (GUILayout.Button("刷新", GUILayout.Height(25)))
        {
            LoadClassesWithAttribute();
        }
    }

    private void SaveToJson()
    {
        if (classDataList.Count == 0)
        {
            EditorUtility.DisplayDialog("提示", "没有可保存的数据", "确定");
            return;
        }

        try
        {
            ClassPriorityListWrapper wrapper = new ClassPriorityListWrapper
            {
                items = classDataList
            };
            string json = LITJson.JsonMapper.ToJson(wrapper, true);
            string path = string.Format(jsonFilePath, attributeName);
            string fullPath = Path.Combine(Application.dataPath, path.Replace("Assets/", ""));
            fullPath = Path.GetFullPath(fullPath);
            File.WriteAllText(fullPath, json);

            Debug.Log($"配置已保存到: {fullPath}");
            EditorUtility.DisplayDialog("成功", $"配置已保存到:\n{fullPath}", "确定");
            LoadClassesWithAttribute();
            AssetDatabase.Refresh();
        }
        catch (Exception e)
        {
            Debug.LogError($"保存失败: {e.Message}");
            EditorUtility.DisplayDialog("错误", $"保存失败:\n{e.Message}", "确定");
        }
    }
}
