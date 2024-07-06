using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Game.Cfg
{
    public interface ICfgList : IBinarySerialize
    {

    }

    public abstract class CfgListBase<T> : ICfgList where T : class, ICfg
    {
        public List<T> CfgList { get; protected set; }

        public abstract void Deserialize(BinaryReader reader);

        public abstract void Serialize(BinaryWriter writer);

        public virtual T Find(Func<T, bool> func)
        {
            return CfgList.Find(x => func(x));
        }

        public virtual List<T> FindAll(Func<T, bool> func)
        {
            var r = CfgList.FindAll(x => func(x));
            return r;
        }
    }
}

