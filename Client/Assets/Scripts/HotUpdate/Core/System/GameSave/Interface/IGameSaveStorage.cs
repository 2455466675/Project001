namespace GameFramework.Core
{
    /// <summary>
    /// 存档持久化后端抽象。把"数据如何落盘"从存档系统中隔离出来,
    /// 使 GameSaveSystem 不直接依赖任何具体存储方案(ES3、PlayerPrefs、自定义二进制、云存档等)。
    /// 更换存储方式时只需新增实现并注入,无需改动存档系统与上层模块。
    /// </summary>
    public interface IGameSaveStorage
    {
        /// <summary>
        /// 将存档数据写入指定文件的指定键。
        /// </summary>
        void Save(string fileName, string key, IGameSaveData data);

        /// <summary>
        /// 读取指定文件指定键的存档数据。文件或键不存在时应返回空数据而非抛异常,
        /// 由调用方按空数据处理缺省逻辑。
        /// </summary>
        IGameSaveData Load(string fileName, string key);

        /// <summary>
        /// 删除指定文件中的某个键。键不存在时应安全跳过。
        /// </summary>
        void Delete(string fileName, string key);

        /// <summary>
        /// 指定文件中是否存在某个键。
        /// </summary>
        bool Exists(string fileName, string key);

        /// <summary>
        /// 读取整个文件的原始字节。文件不存在返回 null。
        /// 供云存档把单档文件当不透明 blob 上传,绕开序列化格式。
        /// </summary>
        byte[] ReadRaw(string fileName);

        /// <summary>
        /// 用原始字节整体覆盖一个文件。
        /// 供云存档把下载下来的 blob 还原成本地档案文件。
        /// </summary>
        void WriteRaw(string fileName, byte[] bytes);
    }
}
