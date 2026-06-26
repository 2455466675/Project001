namespace GameFramework.Core
{
    /// <summary>
    /// 存档摘要处理器。负责维护各存档槽位的概览信息(等级、金币、时间等),
    /// 与具体玩法数据分离,使存档列表界面无需加载完整存档即可展示。
    /// </summary>
    public interface IGameSaveSummary
    {
        IGameSaveData Save(int index);
        IGameSaveData Delete(int index);
        void Load(IGameSaveData data);
    }
}
