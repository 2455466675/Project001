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
    }
}
