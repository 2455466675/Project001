using System;
using System.Collections.Generic;

namespace GameFramework
{
    [Serializable]
    public class KeyValueObjectData
    {
        public string key;
        public string value;
    }

    [Serializable]
    public class KeyValueObjectListWrapper
    {
        public List<KeyValueObjectData> items;
    }
}
