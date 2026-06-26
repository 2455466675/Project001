using System.Collections.Generic;

namespace GameFramework.Core
{
    public interface IReader
    {
        int ReadInt(string key);
        long ReadLong(string key);
        bool ReadBool(string key);
        float ReadFloat(string key);
        double ReadDouble(string key);
        string ReadString(string key);
        T ReadData<T>(string key) where T : class, ISavableData;
        List<T> ReadList<T>(string key) where T : class, ISavableData; 
        Dictionary<int, T> ReadDicWithIntKey<T>(string key) where T : class, ISavableData;
        Dictionary<string, T> ReadDicWithStringKey<T>(string key) where T : class, ISavableData;
    }
}