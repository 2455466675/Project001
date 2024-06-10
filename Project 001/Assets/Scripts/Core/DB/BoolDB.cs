namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class BoolDB : DataBase
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
    }
}

