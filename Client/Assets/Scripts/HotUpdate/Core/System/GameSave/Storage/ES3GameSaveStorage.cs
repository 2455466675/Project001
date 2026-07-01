namespace GameFramework.Core
{
    /// <summary>
    /// 基于 Easy Save 3 (ES3) 的存档持久化实现。
    /// 所有对 ES3 的直接调用都收敛在此类,替换存储方案时不会波及存档系统其余部分。
    /// </summary>
    internal class ES3GameSaveStorage : IGameSaveStorage
    {
        public void Save(string fileName, string key, IGameSaveData data)
        {
            // ES3 需要具体类型才能序列化;GameSaveData 是框架统一的存档容器
            ES3.Save(key, data as GameSaveData, fileName);
        }

        public IGameSaveData Load(string fileName, string key)
        {
            // 带默认值的重载在"文件或键不存在"时返回空对象,避免键缺失时 ES3 抛 KeyNotFoundException
            return ES3.Load<GameSaveData>(key, fileName, new GameSaveData());
        }

        public void Delete(string fileName, string key)
        {
            // 仅在文件存在时调用,省去对不存在文件的无谓 IO;DeleteKey 对不存在的键本身也是安全的
            if (ES3.FileExists(fileName))
            {
                ES3.DeleteKey(key, fileName);
            }
        }

        public bool Exists(string fileName, string key)
        {
            return ES3.FileExists(fileName) && ES3.KeyExists(key, fileName);
        }

        public byte[] ReadRaw(string fileName)
        {
            if (!ES3.FileExists(fileName))
            {
                return null;
            }
            // LoadRawBytes 返回解压/解密后的整份文件内容,正好作为可跨平台传输的 blob
            return ES3.LoadRawBytes(fileName);
        }

        public void WriteRaw(string fileName, byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return;
            }
            // SaveRaw 是整文件覆盖且带备份提交,断电也不会留下半截坏档
            ES3.SaveRaw(bytes, fileName);
        }
    }
}
