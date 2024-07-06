using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Game.Core;
using System.IO.Pipes;

namespace Game.Cfg
{
    /// <summary>
    /// ”Œœ∑ ˝æ›≈‰÷√
    /// </summary>
    public class GameCfg : MonoBehaviour, ICore
    {
        private GameCfgData data;

        public void Awake()
        {}

        public IEnumerator Init()
        {
            using (FileStream stream = new FileStream(Application.streamingAssetsPath + "/cfg.bytes", FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    data = new GameCfgData();
                    data.Deserialize(br);
                }              
            }
            yield return null;
        }

        public T FindById<T>(int id) where T : class, ICfg
        {
            return Find<T>(c => c.Id == id);
        }

        public T Find<T>(Func<T, bool> func) where T : class, ICfg
        {
            Type t = typeof(T);
            if (!data.CfgDatas.ContainsKey(t))
            {
                return null;
            }
            var list = data.CfgDatas[t] as CfgListBase<T>;
            return list.Find(func);
        }

        public List<T> FindAll<T>() where T : class, ICfg
        {
            Type t = typeof(T);
            if (!data.CfgDatas.ContainsKey(t))
            {
                return null;
            }

            var list = data.CfgDatas[t] as CfgListBase<T>;
            return list.CfgList;
        }

        public List<T> FindAll<T>(Func<T, bool> func) where T : class, ICfg
        {
            Type t = typeof(T);
            if (!data.CfgDatas.ContainsKey(t))
            {
                return null;
            }

            var list = data.CfgDatas[t] as CfgListBase<T>;
            return list.FindAll(func);
        }
    }
}
