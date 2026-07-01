namespace GameFramework.Core
{
    /// <summary>
    /// 可被序列化的存档数据单元。由具体数据对象实现,自行决定字段如何读写,
    /// 从而让存档格式与数据结构解耦。
    /// </summary>
    public interface ISavableData
    {
        void OnWrite(IWriter writer);
        void OnRead(IReader reader);
    }
}
