using UnityEngine;
using YooAsset;
using GameFramework.Core;
using Cysharp.Threading.Tasks;
using System.Linq;
using System.Reflection;
using System;

public class GameInitiator : MonoBehaviour
{
    [SerializeField]
    private string packageName = "DefaultPackage";

    [SerializeField]
    private EPlayMode playMode;

    private void Awake()
    {

    }

    private async void Start()
    {
        AssetInitiator assetInitiator = new AssetInitiator(new YooAssetInitiator(packageName, playMode));

        await assetInitiator;


        Assembly assembly;
#if UNITY_EDITOR
        assembly = AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate_Core");
#else
        var handle = YooAssets.LoadAssetAsync("Assets/Bundles/Dlls/HotUpdate_Core.dll");
        await handle;

        TextAsset textAsset = handle.GetAssetObject<TextAsset>();
        assembly = Assembly.Load(textAsset.bytes);
#endif
        Type game = assembly.GetType("GameFramework.Game");
        game.GetMethod("Start").Invoke(null, null);
    }
}
