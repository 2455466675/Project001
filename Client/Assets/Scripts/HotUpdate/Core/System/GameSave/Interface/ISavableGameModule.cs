namespace GameFramework.Core
{
    /// <summary>
    /// 参与整局存档的游戏模块。系统在存/读档时按模块分组回调,
    /// 模块只关心自身数据,不感知底层存储与序列化细节。
    /// </summary>
    public interface ISavableGameModule
    {
        void OnSaveGame(IWriter writer);
        void OnLoadGame(IReader reader);
    }
}
