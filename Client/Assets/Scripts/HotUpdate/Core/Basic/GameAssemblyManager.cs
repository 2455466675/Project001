using System;
using System.Reflection;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HybridCLR;
using UnityEngine;
using LITJson;

namespace GameFramework.Core
{
    public class GameTypeItem
    {
        public int priority;
        public Type type;
    }

    internal class GameAssemblyManager
    {
        private static List<string> AotDllList = new List<string>
        {
            "mscorlib",
            "System",
            "System.Core",
            "UniTask",
            "Unity.InputSystem",
            "UnityEngine.CoreModule",
            "YooAsset",
        };

        private static List<string> HotUpdateDllList = new List<string>
        {
            "GameConfig",
            "GF_HotUpdate_Utility",
            "GF_HotUpdate_Core",
            "GF_HotUpdate_Logic",
            "GF_HotUpdate_View",
        };

        private static string DllPathFormat = "Assets/Bundles/Dlls/{0}.dll";

        private List<Assembly> assemblies;
        private Dictionary<Type, List<GameTypeItem>> attributeMap;

        public async UniTask Init()
        {
            await LoadMetadataForAOTAssembly();
            await LoadHotUpdateAssemblies();
            await InitGameAttributeTypes();
        }

        public GameTypeItem[] GetTypesByGameAttribute<T>() where T : GameAttribute
        {
            Type type = typeof(T);
            if (attributeMap.ContainsKey(type))
            {
                return attributeMap[type].ToArray();
            }
            else
            {
                return new GameTypeItem[0];
            }
        }

        private async UniTask LoadMetadataForAOTAssembly()
        {
#if UNITY_EDITOR
            await UniTask.CompletedTask;
#else           
            HomologousImageMode mode = HomologousImageMode.SuperSet;
            foreach (var aotDllName in AotDllList)
            {
                string path = GetDllPath(aotDllName);
                TextAsset textAsset = await Game.ResourcesManager.LoadAssetAsync<TextAsset>(path);
                if (textAsset != null )
                {
                    LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(textAsset.bytes, mode);
                    Debug.Log($"LoadMetadataForAOTAssembly : {aotDllName}. ret:{err}");
                }
                else
                {
                    Debug.LogError("LoadMetadataForAOTAssembly Fail : " + path);
                }
            }
#endif
        }

        private async UniTask LoadHotUpdateAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>();

#if UNITY_EDITOR
            foreach (var name in HotUpdateDllList)
            {
                Assembly assembly = Assembly.Load(name);
                assemblies.Add(assembly);
                Debug.Log("Load Assembly : " + assembly.GetName().Name);
            }
            await UniTask.CompletedTask;
#else
            foreach (var name in HotUpdateDllList)
            {
                string path = GetDllPath(name);
                TextAsset textAsset = await Game.ResourcesManager.LoadAssetAsync<TextAsset>(path);
 
                if (textAsset != null)
                {
                    Assembly assembly = Assembly.Load(textAsset.bytes);
                    assemblies.Add(assembly);
                    Debug.Log("Load Assembly : " + assembly.GetName().Name);
                }
                else
                {
                    Debug.LogError("Load Assembly Fail : " + path);
                }
            }
#endif
            this.assemblies = assemblies;
        }

        private async UniTask InitGameAttributeTypes()
        {
            Dictionary<string, int> priorityDatas = await GetGamePriorityDatas();
            Dictionary<Type, List<GameTypeItem>> attributeMap = new Dictionary<Type, List<GameTypeItem>>();

            foreach (var assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    var attributes = type.GetCustomAttributes(typeof(GameAttribute), false);
                    foreach (var attribute in attributes)
                    {
                        Type attributeType = attribute.GetType();
                        if (!attributeMap.TryGetValue(attributeType, out List<GameTypeItem> list))
                        {
                            list = new List<GameTypeItem>();
                            attributeMap.Add(attributeType, list);
                        }

                        priorityDatas.TryGetValue(type.FullName, out int priority);
                        list.Add(new GameTypeItem() { priority = priority, type = type });
                    }
                }
            }

            this.attributeMap = attributeMap;
        }

        private string GetDllPath(string dllName)
        {
            string path = string.Format(DllPathFormat, dllName);
            return path;
        }

        private async UniTask<Dictionary<string, int>> GetGamePriorityDatas()
        {
            Dictionary<string, int> datas = new Dictionary<string, int>();
            List<string> fileNames = new List<string>() { "GameSystemAttribute", "GameplayAttribute" };
            foreach (var name in fileNames)
            {
                string path = string.Format("Assets/Bundles/Common/{0}.json", name);
                TextAsset textAsset = await Game.Assets.LoadAssetAsync<TextAsset>(path);
                if (textAsset != null)
                {
                    ClassPriorityListWrapper wrapper = JsonMapper.ToObject<ClassPriorityListWrapper>(textAsset.text);
                    List<ClassPriorityData> items = (wrapper != null && wrapper.items != null) ? wrapper.items : new List<ClassPriorityData>();

                    foreach (var item in items)
                    {
                        datas[item.typeFullName] = item.priority;
                    }
                    Game.Assets.ReleaseAsset(textAsset);
                }
            }
            return datas;
        }
    }
}