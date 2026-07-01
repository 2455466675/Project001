namespace GameFramework.Logic
{
    public class GameSaveSlot : DataModel
    {
        private int index;
        public int Index 
        {
            get
            {
                return index;
            }
            set
            {
                SetValue(ref index, value);
            }
        }

        private int state; //0 = Empty
        public int State
        {
            get
            {
                return state;
            }
            set
            {
                SetValue(ref state, value);
            }
        }

        private long lastTime;
        public long LastTime
        {
            get
            {
                return lastTime;
            }
            set
            {
                SetValue(ref lastTime, value);
            }
        }

        private int level;
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                SetValue(ref level, value);
            }
        }

        private int money;
        public int Money
        {
            get
            {
                return money;
            }
            set
            {
                SetValue(ref money, value);
            }
        }

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
