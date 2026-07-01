using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using YooAsset;

namespace GameFrameworkAOT
{
    public class GameEntryManager
    {
        public string entryAssemblyName;
        public string entryClassName;
        public string entryMethodName;
        public string packageName;

        public GameEntryManager(GameInitConfig config) 
        {
            this.entryAssemblyName = config.entryAssemblyName;
            this.entryClassName = config.entryClassName;
            this.entryMethodName = config.entryMethodName;
            this.packageName = config.packageName;
        }

        public async UniTask Start()
        {
            Debug.Log($"开始启动游戏:{entryAssemblyName}.{entryClassName}.{entryMethodName}");

            Assembly assembly;
#if UNITY_EDITOR
            assembly = AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == entryAssemblyName);
            await UniTask.Yield();
#else
            var handle = YooAssets.LoadAssetAsync($"Assets/Bundles/Dlls/{entryAssemblyName}.dll");
            await handle;

            TextAsset textAsset = handle.GetAssetObject<TextAsset>();
            assembly = Assembly.Load(textAsset.bytes);
#endif
            if (assembly == null)
            {
                Debug.LogError($"assembly is null : {entryAssemblyName}");
                return;
            }

            Type entryClass = assembly.GetType(entryClassName);
            if (entryClass == null)
            {
                Debug.LogError($"entryClass is null : {entryAssemblyName}, {entryClassName}");
                return;
            }

            MethodBase entryMethod = entryClass.GetMethod(entryMethodName);
            if (entryMethod == null)
            {
                Debug.LogError($"entryMethod is null : {entryAssemblyName}, {entryClassName}, {entryMethodName}");
                return;
            }

            // 入口方法签名为 Start(string[])，反射调用时需将参数数组作为单个实参包进 object[]。
            string[] args = new string[] { packageName };
            entryMethod.Invoke(null, new object[] { args });
        }
    }
}
