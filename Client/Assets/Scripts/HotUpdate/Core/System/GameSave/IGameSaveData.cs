using System.Collections.Generic;

namespace GameFramework.Core
{
    public interface IGameSaveData
    {
        public void SetData(Dictionary<string, IGameSaveItem> data);
        public Dictionary<string, IGameSaveItem> GetData();
    }
}