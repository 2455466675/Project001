using System;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class BoolDB : DataBase, IComparable<BoolDB>
    {
        public static implicit operator bool(BoolDB db)
        {
            return db.Value;
        }
        private bool value;
        public bool Value
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
                NotifyChange();
            }
        }
        public BoolDB()
        {
            Value = false;
        }
        public BoolDB(bool v)
        {
            Value = v;
        }
        public override string ToString()
        {
            return Value.ToString();
        }

        public int CompareTo(BoolDB obj)
        {
            return Value.CompareTo(obj.Value);
        }
    }
}

