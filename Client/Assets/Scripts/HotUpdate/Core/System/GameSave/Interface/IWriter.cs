using System.Collections.Generic;

namespace GameFramework.Core
{
    public interface IWriter
    {
        void Write(string key, int value);
        void Write(string key, long value);
        void Write(string key, bool value);
        void Write(string key, float value);
        void Write(string key, double value);
        void Write(string key, string value);
        void Write(string key, ISavableData value);
        void Write<T>(string key, List<T> value) where T : class, ISavableData;
        void Write<T>(string key, Dictionary<int, T> value) where T : class, ISavableData;
        void Write<T>(string key, Dictionary<string, T> value) where T : class, ISavableData;
    }
}