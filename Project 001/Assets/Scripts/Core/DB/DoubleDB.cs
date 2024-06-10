namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class DoubleDB : DataBase
    {
        public static implicit operator double(DoubleDB db)
        {
            return db.Value;
        }
        private double value;
        public double Value
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
                StringValue = value.ToString();
                NotifyChange();
            }
        }
        public DoubleDB()
        {
            Value = 0d;
        }
        public DoubleDB(double v)
        {
            Value = v;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

