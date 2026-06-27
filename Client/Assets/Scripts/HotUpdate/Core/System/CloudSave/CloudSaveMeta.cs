using System;

namespace GameFramework.Core
{
    /// <summary>
    /// 单个云端档位的描述(轻量元数据)。拉清单时只传它即可展示列表,真正的存档 blob 按需下载。
    /// 刻意不含"本地档位 index"——云端档位维护自己的索引,blob 也与档位无关,从而可任意交叉覆盖。
    /// </summary>
    [Serializable]
    public class CloudSaveMeta
    {
        public int cloudIndex;
        public bool isEmpty = true;
        public long timestamp;
        public string deviceName;
        public long version;
        public long byteSize;
        public string checksum;
        public int saveFormatVersion;

        // 游戏自定义的展示数据(json)。后端与框架不解析它,仅原样存取,保证传输层与玩法解耦。
        public string displayInfo;
    }
}
