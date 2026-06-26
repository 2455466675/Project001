using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
{
    [Serializable]
    public class GameSaveData : IGameSaveData
    {
        [SerializeField]
        private Dictionary<string, GameSaveItem> data;

        public Dictionary<string, IGameSaveItem> GetData()
        {
            Dictionary<string, IGameSaveItem> result = new Dictionary<string, IGameSaveItem>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    result[item.Key] = item.Value;
                }
            }
            return result;
        }

        public void SetData(Dictionary<string, IGameSaveItem> data)
        {
            this.data = new Dictionary<string, GameSaveItem>();
            foreach (var item in data)
            {
                this.data[item.Key] = item.Value as GameSaveItem;
            }
        }
    }
}