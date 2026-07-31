# MVC.SourceGen

为 `MVC.ObservableModel` 派生类自动生成带变更通知的属性的 Roslyn 增量源生成器，
用于消除 `字段 + 属性 + SetValue` 的样板代码。

## 目录说明

- 本工程（`MVC.SourceGen~`，`~` 结尾，Unity 自动忽略）是源生成器的**源码**。
- 编译产物是一颗 dll，已部署到 Unity 工程供编译期使用。**dll 才是 Unity 实际用到的东西**，源码工程仅用于维护/演进生成器。
- `bin/`、`obj/` 为编译产物，已在 `.gitignore` 中忽略；源码与 `.csproj` 入库。

## 业务侧用法

```csharp
using MVC;

public partial class GameSaveSlot : DataModel   // 1. 类必须 partial
{
    [ObservableProperty] private int level;      // 2. 字段加特性（using MVC;）
}
```

生成器会补出：

```csharp
public int Level
{
    get => level;
    set => SetValue(ref level, value);
}
```

- 字段名支持 `level` / `_level` / `m_Level` 前缀，统一转成 `Level`。
- 忘写 `partial` 会触发编译错误 `MVCOBS001`。
- 同一 `partial` 类里可混用：需要通知的字段用 `[ObservableProperty]`，
  不需要通知的静态字段保留手写普通属性（参见 `BattleFormationSite`）。
- `[ObservableProperty]` 特性由生成器编译期注入，声明为 `internal`，业务无需自定义。

## 构建与部署

1. 修改生成器逻辑后，在本目录编译：

   ```
   dotnet build -c Release
   ```

2. 将产物覆盖到 Unity 工程的**部署路径**（注意不是 Plugins，而是热更 Logic 目录下）：

   ```
   copy bin\Release\netstandard2.0\MVC.SourceGen.dll ..\Assets\Scripts\HotUpdate\Logic\Analyzers\MVC.SourceGen.dll
   ```

3. 回到 Unity 触发一次重新编译。

## 部署位置与作用域

- 部署路径：`Assets/Scripts/HotUpdate/Logic/Analyzers/MVC.SourceGen.dll`
- dll 的 `.meta` 打了 `RoslynAnalyzer` 标签并禁用所有平台，Unity 按分析器加载。
- 放在 `GF_HotUpdate_Logic` 的 asmdef 目录内，依据 Unity 规则，分析器只作用于
  `GF_HotUpdate_Logic` 及**引用它的程序集**（如 `GF_HotUpdate_View`），
  从而收窄作用域，不波及引擎包、第三方库和 `MVC` 程序集。
- 注意：Unity 预定义程序集（`Assembly-CSharp*`）无论分析器放哪都会应用它，属正常现象，无害。

## 版本

- 目标框架：`netstandard2.0`
- `Microsoft.CodeAnalysis.CSharp` 版本需 ≤ Unity 内置 Roslyn 版本；
  当前 `4.3.0` 对 Unity 6 安全。升级 Unity 后如遇分析器加载告警，再对齐此版本。
