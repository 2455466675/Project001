using System.Collections.Generic;

namespace GameFramework.Core
{
    public interface IReader
    {
        int ReadInt(string key);
        bool ReadBool(string key);
        float ReadFloat(string key);
        double ReadDouble(string key);
        string ReadString(string key);
        T ReadData<T>(string key) where T : class;
        List<T> ReadList<T>(string key) where T : class; 
        Dictionary<int, T> ReadDicWithIntKey<T>(string key) where T : class;
        Dictionary<string, T> ReadDicWithStringKey<T>(string key) where T : class;
    }
}