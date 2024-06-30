using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Game.Core;

namespace Game.Cfg
{
    [Serializable]
    public class CfgData 
    {
        public readonly Dictionary<Type, List<CfgDataBase>> dataMap;
        public CfgData(Dictionary<Type, List<CfgDataBase>> dataMap)
        {
            this.dataMap = dataMap;
        }
    }

    /// <summary>
    /// ”Œœ∑ ˝æ›≈‰÷√
    /// </summary>
    public class GameCfg : MonoBehaviour, ICore
    {
        private CfgData cfgData;
        private Dictionary<Type, List<CfgDataBase>> DataMap => cfgData.dataMap;

        public void Awake()
        {}

        public IEnumerator Init()
        {
            using (FileStream stream = new FileStream(Application.streamingAssetsPath + "/cfgData.txt", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                cfgData = formatter.Deserialize(stream) as CfgData;
            }
            yield return null;
        }

        public T FindById<T>(int id) where T : CfgDataBase
        {
            return Find<T>((c) => c.id == id);
        }

        public T Find<T>(Func<T, bool> func) where T : CfgDataBase
        {
            Type t = typeof(T);
            if (!DataMap.ContainsKey(t))
            {
                return null;
            }
            List<CfgDataBase> list = DataMap[t];
            return (T)list.Find(a => func(a as T));
        }

        public List<T> FindAll<T>() where T : CfgDataBase
        {
            Type t = typeof(T);
            if (!DataMap.ContainsKey(t))
            {
                return null;
            }

            List<CfgDataBase> list = DataMap[t];
            return list.ConvertAll(a => a as T);
        }

        public List<T> FindAll<T>(Func<T, bool> func) where T : CfgDataBase
        {
            List<T> list = FindAll<T>();
            return list.FindAll(a => func(a));
        }
    }
}
