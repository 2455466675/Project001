using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using System.IO;
#endif

namespace GameFramework.Core
{
    [Serializable]
    public class ActionItemObjectData
    {
        public string key;
        public string value;
    }

    [Serializable]
    public class ActionItemObjectListWrapper
    {
        public List<ActionItemObjectData> items;
    }

    [CreateAssetMenu(fileName = "ActionItem", menuName = "Scriptable Objects/ActionItem")]
    public class ActionItem : ScriptableObject
    {
        public string actionName;

        public string desc;

        [AbstractType(typeof(ActionCommand))]
        [SerializeReference]
        public ActionCommand[] commands;

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(actionName))
            {
                actionName = name;
            }    
        }

        [MenuItem("Tools/MyTools/Generate Action Map")]
        public static void Generate()
        {
            string path = "Assets/Bundles/Action";
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(ActionItem).Name}", new[] { path });

            var items = guids.Select(guid =>
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                ActionItem asset = AssetDatabase.LoadAssetAtPath<ActionItem>(assetPath);

                if (asset == null)
                {
                    Debug.LogWarning($"Found non-{typeof(ActionItem).Name} asset at: {assetPath}");
                    return null;
                }

                ActionItemObjectData data = new ActionItemObjectData();
                data.key = asset.actionName;
                data.value = Path.ChangeExtension(assetPath, null);

                return data;
            })
            .Where(asset => asset != null)
            .ToList();

            ActionItemObjectListWrapper wrapper = new ActionItemObjectListWrapper();
            wrapper.items = items;

            string json = LITJson.JsonMapper.ToJson(wrapper, true);

            string fullPath = Path.Combine(Application.dataPath, "Bundles/Common/ActionsMap.json");
            fullPath = Path.GetFullPath(fullPath);
            File.WriteAllText(fullPath, json);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("成功", $"配置已保存到:\n{fullPath}", "确定");
        }
#endif

    }
}
