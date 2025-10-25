using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    [System.Serializable]
    public class ClassPriorityData
    {
        public string type;
        public int priority;
    }

    [System.Serializable]
    public class ClassPriorityListWrapper
    {
        public List<ClassPriorityData> items;
    }
}
