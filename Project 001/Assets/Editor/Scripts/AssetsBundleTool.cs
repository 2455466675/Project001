using Game.Cfg;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class AssetsBundleTool
    {

        [MenuItem("AssestBundleTool/build in windows")]
        public static void BuildInWindows()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "assetBundles");
            path = path.Replace("\\", "/");
            BuildTarget target = BuildTarget.StandaloneWindows64;

            //清空StreamingAssets目录
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);

            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression, target);
 
            Debug.Log($"<color=green>打包完成！位置={path},目标平台={target}</color>");
        }
    }
}