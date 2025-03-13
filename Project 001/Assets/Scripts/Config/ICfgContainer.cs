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

    public abstract class CfgContainerBase<T> : ICfgContainer where T : class, ICfg
    {
        public Dictionary<int, T> CfgMap { get; protected set; }

        public abstract void Deserialize(BinaryReader reader);

        public abstract void Serialize(BinaryWriter writer);

        public virtual T Find(int id)
        {
            if (CfgMap == null)
            {
                return null;
            }
            if (!CfgMap.ContainsKey(id))
            {                
                return null;
            }
            return CfgMap[id];
        }

        public virtual T Find(Func<T, bool> func)
        {
            if (CfgMap == null)
            {
                return null;
            }
            
            return CfgMap.Values.Where(func).FirstOrDefault();
        }

        public virtual List<T> FindAll()
        {
            if (CfgMap == null)
            {
                return null;
            }
            return CfgMap.Values.ToList();
        }

        public virtual List<T> FindAll(Func<T, bool> func)
        {
            if (CfgMap == null)
            {
                return null;
            }
            return CfgMap.Values.Where(func).ToList();
        }
    }
}

