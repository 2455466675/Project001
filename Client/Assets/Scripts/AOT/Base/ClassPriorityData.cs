using System;
using System.Collections.Generic;

namespace GameFramework
{
    [Serializable]
    public class ClassPriorityData
    {
        public string typeFullName;
        public int priority;
    }

    [Serializable]
    public class ClassPriorityListWrapper
    {
        public List<ClassPriorityData> items;
    }
}
