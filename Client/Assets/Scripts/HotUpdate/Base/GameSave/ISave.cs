using System.Collections.Generic;

namespace GameFramework 
{
    public interface ISaveWriter
    {
        void Next(string groupName);
        void Write(string key, int value);
        void Write(string key, bool value);
        void Write(string key, string value);
        void Write(string key, DataModel value);
        void Write(string key, IList<DataModel> value);
        void Write(string key, IReadOnlyDictionary<int, DataModel> value);
        void Write(string key, IReadOnlyDictionary<string, DataModel> value);
    }

    public interface ISaveReader
    {
        void Next(string groupName);
        int ReadInt(string key);
        bool ReadBool(string key);
        string ReadString(string key);
        DataModel ReadData(string key);
        List<DataModel> ReadList(string key);
        Dictionary<int, DataModel> ReadDicWithIntKey(string key);
        Dictionary<string, DataModel> ReadDicWithStringKey(string key);
    }
}