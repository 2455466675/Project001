using System;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class FloatDB : DataBase, IComparable<FloatDB>
    {
        public static implicit operator float(FloatDB db)
        {
            return db.Value;
        }
        private float value;
        public float Value
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
                DoubleValue = value;
                StringValue = value.ToString();
                NotifyChange();
            }
        }
        public FloatDB()
        {
            Value = 0f;
        }
        public FloatDB(float v)
        {
            Value = v;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public int CompareTo(FloatDB db)
        {
            return Value.CompareTo(db.Value);
        }
    }
}

