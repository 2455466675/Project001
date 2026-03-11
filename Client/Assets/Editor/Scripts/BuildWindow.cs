using UnityEditor;
using UnityEngine;
using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using YooAsset.Editor;
using System.IO;
using System.Collections.Generic;
using YooAsset;

public class BuildWindow : EditorWindow
{
    private enum BuildType
    {
        BuildHotUpdateDLL = 0,
        BuildAssetsBundle = 1,
        BuildPlayerPackage = 2,
        BuildHotAssets = 3,
        BuildAll = 4,
    }

    // 基础设置
    private BuildTarget buildTarget = BuildTarget.StandaloneWindows64;
    private string buildVersion = "1.0.0";
    private string packageOutputPath = "";

    // HybridCLR 设置
    private string hybridCLROutputPath = "Assets/Bundles/Dlls";

    // YooAsset 设置
    private string yooAssetPackageName = "DefaultPackage";
    private bool clearBuildCache = false;
    private ECompressOption compressOption = ECompressOption.LZ4;
    private EFileNameStyle fileNameStyle = EFileNameStyle.BundleName_HashName;
    private EBuildinFileCopyOption copyOption = EBuildinFileCopyOption.None;
    private string buildPipeline = "ScriptableBuildPipeline";
    private bool encryptionServices = false;
    private bool useAssetDependencyDB = true;

    // UI 相关
    private Vector2 scrollPosition;
    private bool isBuilding = false;
    private BuildType? pendingBuildType = null;

    private string[] buildPipelineOptions = new string[]
    {
        "BuiltinBuildPipeline",
        "ScriptableBuildPipeline",
        "RawFileBuildPipeline"
    };

    private string[] BuildTypeDesc = new string[]
    {
        "编译热更新DLL",
        "构建AssetBundle资源包",
        "构建整包",
        "构建热更补丁",
        "构建完整包(1、2、3)",
    };

    [MenuItem("Build/BuildWindow")]
    public static void ShowWindow()
    {
        var window = GetWindow<BuildWindow>("打包工具");
        window.minSize = new Vector2(500, 700);
        window.Show();
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("打包工具", EditorStyles.boldLabel);

        if (isBuilding)
        {
            GUILayout.Space(20);
            EditorGUILayout.HelpBox("正在构建中，请稍候...", MessageType.Info);
        }
        else
        {
            // 检查待执行的构建任务
            if (pendingBuildType.HasValue)
            {
                BuildType buildType = pendingBuildType.Value;
                pendingBuildType = null;
                EditorApplication.delayCall += () => ExecuteBuild(buildType);
            }

            DrawSeparator();
            DrawBasicSettings();
            DrawSeparator();
            DrawHybridCLRSettings();
            DrawSeparator();
            DrawYooAssetSettings();
            DrawSeparator();
            DrawBuildButtons();
            DrawSeparator();
            DrawQuickTools();
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawBasicSettings()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("基础设置", EditorStyles.boldLabel);
        GUILayout.Space(5);

        buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("目标平台", buildTarget);
        buildVersion = EditorGUILayout.TextField("版本号", buildVersion);

        EditorGUILayout.HelpBox($"当前平台: {buildTarget}\n版本号: {buildVersion}", MessageType.Info);
        EditorGUILayout.EndVertical();
    }

    private void DrawHybridCLRSettings()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("HybridCLR 设置", EditorStyles.boldLabel);
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        hybridCLROutputPath = EditorGUILayout.TextField("DLL输出路径", hybridCLROutputPath);
        if (GUILayout.Button("浏览", GUILayout.Width(60)))
        {
            string path = EditorUtility.OpenFolderPanel("选择DLL输出路径", hybridCLROutputPath, "");
            if (!string.IsNullOrEmpty(path))
            {
                hybridCLROutputPath = GetRelativePath(path);
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.HelpBox("热更新DLL将被编译到此路径，并自动转换为.bytes格式", MessageType.Info);
        EditorGUILayout.EndVertical();
    }

    private void DrawYooAssetSettings()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("YooAsset 设置", EditorStyles.boldLabel);
        GUILayout.Space(5);

        yooAssetPackageName = EditorGUILayout.TextField("资源包名称", yooAssetPackageName);

        int selectedIndex = System.Array.IndexOf(buildPipelineOptions, buildPipeline);
        if (selectedIndex < 0) selectedIndex = 1;
        selectedIndex = EditorGUILayout.Popup("构建管线", selectedIndex, buildPipelineOptions);
        buildPipeline = buildPipelineOptions[selectedIndex];

        compressOption = (ECompressOption)EditorGUILayout.EnumPopup("压缩选项", compressOption);
        fileNameStyle = (EFileNameStyle)EditorGUILayout.EnumPopup("文件命名风格", fileNameStyle);
        copyOption = (EBuildinFileCopyOption)EditorGUILayout.EnumPopup("复制到StreamingAssets", copyOption);

        clearBuildCache = EditorGUILayout.Toggle("清除构建缓存", clearBuildCache);
        encryptionServices = EditorGUILayout.Toggle("启用加密服务", encryptionServices);
        useAssetDependencyDB = EditorGUILayout.Toggle("Use Asset Dependency DB", useAssetDependencyDB);

        if (encryptionServices)
        {
            EditorGUILayout.HelpBox("请确保已实现 IEncryptionServices 接口", MessageType.Warning);
        }

        GUILayout.Space(5);
        if (GUILayout.Button("打开 YooAsset 资源包配置", GUILayout.Height(30)))
        {
            EditorApplication.ExecuteMenuItem("YooAsset/AssetBundle Collector");
        }

        EditorGUILayout.HelpBox($"资源包将使用 {buildPipeline} 管线构建\n请确保已配置资源收集器（Collector）", MessageType.Info);
        EditorGUILayout.EndVertical();
    }

    private void DrawBuildButtons()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("构建操作", EditorStyles.boldLabel);
        GUILayout.Space(5);

        GUI.enabled = !isBuilding;

        if (GUILayout.Button("编译C#热更DLL", GUILayout.Height(35)))
        {
            RequestBuild(BuildType.BuildHotUpdateDLL);
        }
        EditorGUILayout.HelpBox($"调用HyBridCLR接口,将可热更的C#代码编译为DLL,生成的DLL会拷贝到{hybridCLROutputPath}下", MessageType.Info);

        GUILayout.Space(5);

        if (GUILayout.Button("构建AssetsBundle资源包", GUILayout.Height(35)))
        {
            RequestBuild(BuildType.BuildAssetsBundle);
        }
        EditorGUILayout.HelpBox($"调用YooAsset接口,将收集器中收集的资源打包", MessageType.Info);

        GUILayout.Space(5);

        if (GUILayout.Button("构建整包", GUILayout.Height(35)))
        {
            packageOutputPath = EditorUtility.SaveFolderPanel("构建整包", "Output", string.Empty);
            if (!string.IsNullOrEmpty(packageOutputPath))
            {
                RequestBuild(BuildType.BuildPlayerPackage);                
            }
        }
        EditorGUILayout.HelpBox($"构建整包时请确保AssetBundle已复制到StreamingAssets目录下", MessageType.Info);

        GUILayout.Space(5);

        GUI.backgroundColor = Color.green;

        if (GUILayout.Button("一键打包（编译C#热更DLL、构建AssetsBundle资源包、构建整包）", GUILayout.Height(50)))
        {
            if (copyOption != EBuildinFileCopyOption.ClearAndCopyAll || copyOption != EBuildinFileCopyOption.OnlyCopyAll)
            {
                EditorUtility.DisplayDialog("提示", "请将‘复制到StreamingAssets’设置为ClearAndCopyAll或OnlyCopyAll", "确定");
                return;
            }

            packageOutputPath = EditorUtility.SaveFolderPanel("构建整包", "Output", string.Empty);
            if (!string.IsNullOrEmpty(packageOutputPath))
            {
                RequestBuild(BuildType.BuildAll);
            }
        }
        GUI.backgroundColor = Color.white;

        GUILayout.Space(10);

        GUI.backgroundColor = Color.blue;

        if (GUILayout.Button("构建热更补丁（编译C#热更DLL、构建AssetsBundle资源包）", GUILayout.Height(35)))
        {
            RequestBuild(BuildType.BuildHotAssets);
        }
        GUI.backgroundColor = Color.white;

        GUI.enabled = true;

        if (isBuilding)
        {
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("正在构建中，请稍候...", MessageType.Info);
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawQuickTools()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("快捷工具", EditorStyles.boldLabel);
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("打开构建输出目录", GUILayout.Height(30)))
        {
            string outputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
            if (Directory.Exists(outputRoot))
            {
                EditorUtility.RevealInFinder(outputRoot);
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "输出目录不存在，请先执行构建", "确定");
            }
        }

        if (GUILayout.Button("打开StreamingAssets", GUILayout.Height(30)))
        {
            string streamingAssets = Application.streamingAssetsPath;
            if (Directory.Exists(streamingAssets))
            {
                EditorUtility.RevealInFinder(streamingAssets);
            }
            else
            {
                Directory.CreateDirectory(streamingAssets);
                EditorUtility.RevealInFinder(streamingAssets);
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("打开DLL输出目录", GUILayout.Height(30)))
        {
            string dllPath = Path.Combine(Application.dataPath.Replace("Assets", ""), hybridCLROutputPath);
            if (Directory.Exists(dllPath))
            {
                EditorUtility.RevealInFinder(dllPath);
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "DLL输出目录不存在，请先编译热更新DLL", "确定");
            }
        }

        if (GUILayout.Button("打开整包输出目录", GUILayout.Height(30)))
        {
            string buildsPath = packageOutputPath;
            if (!string.IsNullOrEmpty(buildsPath) && Directory.Exists(buildsPath))
            {
                EditorUtility.RevealInFinder(buildsPath);
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "整包输出目录不存在，请先构建整包", "确定");
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void DrawSeparator()
    {
        GUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(10);
    }

    private void RequestBuild(BuildType buildType)
    {
        string desc = BuildTypeDesc[(int)buildType];

        if (buildType == BuildType.BuildAll || buildType == BuildType.BuildPlayerPackage) 
        {
            desc = $"将执行：{desc}\n输出位置：{packageOutputPath}\n是否继续？";
        }
        else
        {
            desc = $"将执行：{desc}\n\n是否继续？";
        }

        if (EditorUtility.DisplayDialog("确认", desc, "确定", "取消"))
        {
            pendingBuildType = buildType;
            Repaint();
        }
    }

    private void ExecuteBuild(BuildType buildType)
    {
        string desc = BuildTypeDesc[(int)buildType];
        isBuilding = true;

        try
        {
            Debug.Log($"========== 开始{desc} ==========");

            switch (buildType)
            {
                case BuildType.BuildHotUpdateDLL:
                    BuildHotUpdateDLL();
                    break;
                case BuildType.BuildAssetsBundle:
                    BuildAssetBundle();
                    break;
                case BuildType.BuildPlayerPackage:
                    BuildPlayerPackage();
                    break;
                case BuildType.BuildHotAssets:
                    BuildHotUpdateDLL();
                    BuildAssetBundle();
                    break;
                case BuildType.BuildAll:
                    BuildHotUpdateDLL();
                    BuildAssetBundle();
                    BuildPlayerPackage();
                    break;
            }

            Debug.Log($"========== {desc}完成 ==========");
            EditorUtility.DisplayDialog("成功", $"{desc}完成！", "确定");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"{desc}失败: {e.Message}\n{e.StackTrace}");
            EditorUtility.DisplayDialog("失败", $"{desc}失败:\n{e.Message}", "确定");
        }
        finally
        {
            isBuilding = false;
            Repaint();
        }
    }

    // ========== 构建方法 ==========

    private IEncryptionServices CreateEncryptionInstance()
    {
        return null;
    }

    private void BuildHotUpdateDLL()
    {
        Debug.Log("===== 1. 编译热更新DLL =====");

        CompileDllCommand.CompileDll(buildTarget);
        Debug.Log("✓ 编译DLL完成");

        PrebuildCommand.GenerateAll();
        Debug.Log("✓ 生成AOT补充元数据完成");

        CopyHotUpdateDLLs();
        Debug.Log("✓ 复制DLL完成");
    }

    private void BuildAssetBundle()
    {
        Debug.Log("===== 2. 构建AssetBundle资源包 =====");

        BuildParameters buildParameters = CreateBuildParameters();

        if (buildParameters == null)
        {
            throw new System.Exception("创建构建参数失败，请检查构建管线设置");
        }

        Debug.Log($"构建参数：\n" +
                  $"- 包名: {buildParameters.PackageName}\n" +
                  $"- 版本: {buildParameters.PackageVersion}\n" +
                  $"- 平台: {buildParameters.BuildTarget}\n" +
                  $"- 管线: {buildParameters.BuildPipeline}\n" +
                  $"- 压缩: {compressOption}\n" +
                  $"- 输出目录: {buildParameters.BuildOutputRoot}");

        IBuildPipeline pipeline = CreateBuildPipeline();
        var buildResult = pipeline.Run(buildParameters, true);

        if (buildResult.Success)
        {
            Debug.Log("✓ AssetBundle构建完成");
            Debug.Log($"- 输出目录: {buildParameters.BuildOutputRoot}");
        }
        else
        {
            throw new System.Exception($"YooAsset 构建失败: {buildResult.ErrorInfo}");
        }
    }

    private BuildParameters CreateBuildParameters()
    {
        string buildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
        string buildinFileRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot();

        BuildParameters buildParameters = null;

        if (buildPipeline == "BuiltinBuildPipeline")
        {
            var parameters = new BuiltinBuildParameters();
            parameters.BuildOutputRoot = buildOutputRoot;
            parameters.BuildinFileRoot = buildinFileRoot;
            parameters.BuildPipeline = EBuildPipeline.BuiltinBuildPipeline.ToString();
            parameters.BuildTarget = buildTarget;
            parameters.PackageName = yooAssetPackageName;
            parameters.PackageVersion = buildVersion;
            parameters.EnableSharePackRule = true;
            parameters.VerifyBuildingResult = true;
            parameters.CompressOption = compressOption;
            parameters.FileNameStyle = fileNameStyle;
            parameters.BuildinFileCopyOption = copyOption;
            parameters.BuildinFileCopyParams = yooAssetPackageName;
            parameters.UseAssetDependencyDB = useAssetDependencyDB;

            if (encryptionServices)
            {
                parameters.EncryptionServices = CreateEncryptionInstance();
            }

            buildParameters = parameters;
        }
        else if (buildPipeline == "ScriptableBuildPipeline")
        {
            var parameters = new ScriptableBuildParameters();
            parameters.BuildOutputRoot = buildOutputRoot;
            parameters.BuildinFileRoot = buildinFileRoot;
            parameters.BuildPipeline = EBuildPipeline.ScriptableBuildPipeline.ToString();
            parameters.BuildBundleType = (int)EBuildBundleType.AssetBundle;
            parameters.BuildTarget = buildTarget;
            parameters.PackageName = yooAssetPackageName;
            parameters.PackageVersion = buildVersion;
            parameters.EnableSharePackRule = true;
            parameters.VerifyBuildingResult = true;
            parameters.CompressOption = compressOption;
            parameters.FileNameStyle = fileNameStyle;
            parameters.BuildinFileCopyOption = copyOption;
            parameters.BuildinFileCopyParams = yooAssetPackageName;
            parameters.UseAssetDependencyDB = useAssetDependencyDB;
            parameters.BuiltinShadersBundleName = GetBuiltinShaderBundleName();

            if (encryptionServices)
            {
                parameters.EncryptionServices = CreateEncryptionInstance();
            }

            buildParameters = parameters;
        }
        else if (buildPipeline == "RawFileBuildPipeline")
        {
            var parameters = new RawFileBuildParameters();
            parameters.BuildOutputRoot = buildOutputRoot;
            parameters.BuildinFileRoot = buildinFileRoot;
            parameters.BuildPipeline = EBuildPipeline.RawFileBuildPipeline.ToString();
            parameters.BuildTarget = buildTarget;
            parameters.PackageName = yooAssetPackageName;
            parameters.PackageVersion = buildVersion;
            parameters.EnableSharePackRule = true;
            parameters.VerifyBuildingResult = true;
            parameters.FileNameStyle = fileNameStyle;
            parameters.BuildinFileCopyOption = copyOption;
            parameters.BuildinFileCopyParams = yooAssetPackageName;
            parameters.UseAssetDependencyDB = useAssetDependencyDB;

            if (encryptionServices)
            {
                parameters.EncryptionServices = CreateEncryptionInstance();
            }

            buildParameters = parameters;
        }

        return buildParameters;
    }

    private IBuildPipeline CreateBuildPipeline() 
    {
        IBuildPipeline pipeline = null;
        if (buildPipeline == "BuiltinBuildPipeline")
        {
            pipeline = new BuiltinBuildPipeline();
        }
        if (buildPipeline == "ScriptableBuildPipeline")
        {
            pipeline = new ScriptableBuildPipeline();
        }
        if (buildPipeline == "RawFileBuildPipeline")
        {
            pipeline = new RawFileBuildPipeline();
        }
        return pipeline;
    }

    private string GetBuiltinShaderBundleName()
    {
        var uniqueBundleName = AssetBundleCollectorSettingData.Setting.UniqueBundleName;
        var packRuleResult = DefaultPackRule.CreateShadersPackRuleResult();
        return packRuleResult.GetBundleName(yooAssetPackageName, uniqueBundleName);
    }

    private void BuildPlayerPackage()
    {
        Debug.Log("===== 3. 构建整包 =====");

        string outputPath = GetPlayerOutputPath();
        Debug.Log($"输出路径: {outputPath}");

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = GetEnabledScenes();
        buildPlayerOptions.locationPathName = outputPath;
        buildPlayerOptions.target = buildTarget;
        buildPlayerOptions.options = BuildOptions.None;

        Debug.Log($"开始构建整包，包含 {buildPlayerOptions.scenes.Length} 个场景");

        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log($"✓ 整包构建成功");
            Debug.Log($"- 输出路径: {outputPath}");
            Debug.Log($"- 构建大小: {FormatBytes(report.summary.totalSize)}");
            Debug.Log($"- 构建时间: {report.summary.totalTime}");

            EditorUtility.RevealInFinder(outputPath);
        }
        else
        {
            throw new System.Exception($"整包构建失败: {report.summary.result}");
        }
    }

    // ========== 辅助方法 ==========

    private void CopyHotUpdateDLLs()
    {
        string fullOutputPath = Path.Combine(Application.dataPath.Replace("Assets", ""), hybridCLROutputPath);
        if (!Directory.Exists(fullOutputPath))
        {
            Directory.CreateDirectory(fullOutputPath);
        }

        CopyAOTAssemblies();
        CopyHotUpdateAssemblies();
        AssetDatabase.Refresh();
    }

    private void CopyAOTAssemblies()
    {
        var target = EditorUserBuildSettings.activeBuildTarget;
        string aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
        string aotAssembliesDstDir = hybridCLROutputPath;

        int count = 0;
        foreach (var dll in SettingsUtil.AOTAssemblyNames)
        {
            string srcDllPath = $"{aotAssembliesSrcDir}/{dll}.dll";
            if (!File.Exists(srcDllPath))
            {
                Debug.LogWarning($"AOT补充元数据dll不存在: {srcDllPath}");
                continue;
            }
            string dllBytesPath = $"{aotAssembliesDstDir}/{dll}.dll.bytes";
            File.Copy(srcDllPath, dllBytesPath, true);
            Debug.Log($"[AOT] {dll}.dll -> {dll}.dll.bytes");
            count++;
        }
        Debug.Log($"✓ 复制了 {count} 个AOT补充元数据DLL");
    }

    private void CopyHotUpdateAssemblies()
    {
        var target = EditorUserBuildSettings.activeBuildTarget;

        string hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
        string hotfixAssembliesDstDir = hybridCLROutputPath;

        int count = 0;
        foreach (var dll in SettingsUtil.HotUpdateAssemblyFilesExcludePreserved)
        {
            string dllPath = $"{hotfixDllSrcDir}/{dll}";
            string dllBytesPath = $"{hotfixAssembliesDstDir}/{dll}.bytes";
            File.Copy(dllPath, dllBytesPath, true);
            Debug.Log($"[HotUpdate] {dll} -> {dll}.bytes");
            count++;
        }
        Debug.Log($"✓ 复制了 {count} 个热更新DLL");
    }

    private string[] GetEnabledScenes()
    {
        var scenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                scenes.Add(scene.path);
            }
        }
        return scenes.ToArray();
    }

    private string GetPlayerOutputPath()
    {
        string extension = "";
        switch (buildTarget)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                extension = ".exe";
                break;
            case BuildTarget.Android:
                extension = ".apk";
                break;
            case BuildTarget.iOS:
                extension = "";
                break;
            case BuildTarget.StandaloneOSX:
                extension = ".app";
                break;
        }
        
        string folderName = $"{Application.productName}_{buildTarget}_{buildVersion}";
        string outputDir = Path.Combine(packageOutputPath, folderName);

        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        return Path.Combine(outputDir, Application.productName + extension);
    }

    private string GetRelativePath(string fullPath)
    {
        string dataPath = Application.dataPath;
        if (fullPath.StartsWith(dataPath))
        {
            return "Assets" + fullPath.Substring(dataPath.Length).Replace("\\", "/");
        }
        return fullPath;
    }

    private string FormatBytes(ulong bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return string.Format("{0:0.##} {1}", len, sizes[order]);
    }
}
