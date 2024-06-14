using System;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class IntDB : DataBase, IComparable<IntDB>
    {
        public static implicit operator int(IntDB db)
        {
            return db.Value;
        }
        private int value;
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value == value)
                {
                    return;
                }
                this.value = value;
                IntValue = value;
                FloatValue = value;
                DoubleValue = value;
                StringValue = value.ToString();
                NotifyChange();
            }
        }
        public IntDB()
        {
            Value = 0;
        }
        public IntDB(int v)
        {
            Value = v;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public int CompareTo(IntDB db)
        {
            return Value.CompareTo(db.Value);
        }
    }
}

