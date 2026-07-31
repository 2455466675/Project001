using MVC;

namespace GameFramework.Logic
{
    public partial class GameSaveSlot : DataModel
    {
        [ObservableProperty] private int index;
        [ObservableProperty] private int state; //0 = Empty
        [ObservableProperty] private long lastTime;
        [ObservableProperty] private int level;
        [ObservableProperty] private int money;

        /// <summary>
        /// 导出摘要内容。与 ApplySummary 一起,是可观察模型与摘要 DTO 之间唯一的字段映射点;
        /// 新增摘要项时只需在此补一行,持久化/云传输等调用方无需改动。
        /// </summary>
        public SaveSlotSummary ExportSummary()
        {
            return new SaveSlotSummary
            {
                hasData = state != 0,
                level = Level,
                money = Money,
                lastTime = LastTime,
            };
        }

        public void ApplySummary(SaveSlotSummary summary)
        {
            if (summary == null)
            {
                return;
            }
            State = summary.hasData ? 1 : 0;
            Level = summary.level;
            Money = summary.money;
            LastTime = summary.lastTime;
        }
    }
}
