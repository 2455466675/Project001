namespace GameFramework.Core
{
    /// <summary>
    /// 存档摘要处理器。负责维护各存档槽位的概览信息(等级、金币、时间等),
    /// </summary>
    public interface IGameSaveSummary
    {
        void Save(int index);
        void Delete(int index);
        void Load(IGameSaveData data);
        void JsonToSummary(int index, string json);
        string SummaryToJson(int index);

        /// <summary>
        /// 把当前摘要快照打包成可落盘的数据
        /// </summary>
        IGameSaveData Capture();
    }
}
