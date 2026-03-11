using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Config
{
    public interface ICfgContainer : IBinarySerialize
    {

    }

    public abstract class CfgContainerBase<T> : ICfgContainer where T : class, IConfig
    {
        public Dictionary<int, T> IntCfgMap { get; protected set; }

        public Dictionary<string, T> StrCfgMap { get; protected set; }

        public abstract void Deserialize(BinaryReader reader);

        public abstract void Serialize(BinaryWriter writer);

        public T Find(int id)
        {
            if (IntCfgMap == null)
            {
                return default;
            }
            if (!IntCfgMap.ContainsKey(id))
            {
                return default;
            }
            return IntCfgMap[id];
        }

        public T Find(string id)
        {
            if (StrCfgMap == null)
            {
                return null;
            }
            if (!StrCfgMap.ContainsKey(id))
            {
                return null;
            }
            return StrCfgMap[id];
        }

        public T Find(Func<T, bool> func)
        {
            Type type = typeof(T);
            if (typeof(IConfig_IntKey).IsAssignableFrom(type) && IntCfgMap != null)
            {
                return IntCfgMap.Values.Where(func).FirstOrDefault();
            }
            if (typeof(IConfig_StringKey).IsAssignableFrom(type) && StrCfgMap != null)
            {
                return StrCfgMap.Values.Where(func).FirstOrDefault();
            }
            return default;
        }

        public T[] FindAll()
        {
            Type type = typeof(T);
            if (typeof(IConfig_IntKey).IsAssignableFrom(type) && IntCfgMap != null)
            {
                return IntCfgMap.Values.ToArray();
            }
            if (typeof(IConfig_StringKey).IsAssignableFrom(type) && StrCfgMap != null)
            {
                return StrCfgMap.Values.ToArray();
            }
            return new T[0];
        }

        public T[] FindAll(Func<T, bool> func)
        {
            Type type = typeof(T);
            if (typeof(IConfig_IntKey).IsAssignableFrom(type) && IntCfgMap != null)
            {
                return IntCfgMap.Values.Where(func).ToArray();
            }
            if (typeof(IConfig_StringKey).IsAssignableFrom(type) && StrCfgMap != null)
            {
                return StrCfgMap.Values.Where(func).ToArray();
            }
            return new T[0];
        }
    }
}

