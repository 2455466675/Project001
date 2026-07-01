using System;

namespace GameFramework.Logic
{
    /// <summary>
    /// 存档槽位的摘要内容(供存档列表展示与云端传输)。
    /// 新增摘要项时只需在此处加字段,持久化、上传/下载、SetSlotSummary 等调用方都不受影响。
    /// </summary>
    [Serializable]
    public class SaveSlotSummary
    {
        public bool hasData;
        public int level;
        public int money;
        public long lastTime;
    }
}
