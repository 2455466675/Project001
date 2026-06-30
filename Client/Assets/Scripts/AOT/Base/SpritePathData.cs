using System;
using System.Collections.Generic;

namespace GameFramework
{
    [Serializable]
    public class SpritePathData
    {
        public bool isMultiple;
        public string spriteName;
        public string assetPath;
    }

    [Serializable]
    public class SpritePathListWrapper
    {
        public List<SpritePathData> items;
    }
}