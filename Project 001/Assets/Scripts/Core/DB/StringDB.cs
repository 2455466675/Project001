namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class StringDB : DataBase
    {
        public static implicit operator string(StringDB db)
        {
            return db.Value;
        }
        private string value;
        public string Value
        {
            get
            {
                return value;
            }
            set
            {
                if (string.Equals(this.value, value))
                {
                    return;
                }
                this.value = value;
                NotifyChange();
            }
        }
        public StringDB()
        {
            Value = string.Empty;
        }
        public StringDB(string v)
        {
            Value = v;
        }
        public override string ToString()
        {
            return Value;
        }
    }
}

