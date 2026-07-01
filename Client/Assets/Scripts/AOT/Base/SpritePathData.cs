using System;
using System.Collections.Generic;

namespace GameFramework
{
    [Serializable]
    public class SpritePathData
    {
        // 所有精灵资源都位于该目录下，故只存储相对路径以精简数据，运行时再拼接根路径还原。
        public const string RootPath = "Assets/Bundles/ArtResources";

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