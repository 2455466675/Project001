using System;
using System.Collections.Generic;

namespace GameFramework.Core 
{
    [Serializable]
    public class SaveData
    {
        public Dictionary<string, SaveItem> items;
    }
}