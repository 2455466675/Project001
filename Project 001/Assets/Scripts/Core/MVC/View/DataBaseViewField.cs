using System;

namespace MVC
{
    /// <summary>
    /// 
    /// </summary>
    public class DataBaseViewField
    {
        public bool IsNull => db == null;

        private DataBase db;
        private Action action;
        public DataBaseViewField(DataBase db)
        {
            this.db = db;
        }

        public void Bind(Action action)
        {
            if (action == null)
            {
                return;
            }
            this.action = action;
            db.Bind(OnValueChanged);
        }

        public void Unbind()
        {
            db.Unbind(OnValueChanged);
            db = null;
            action = null;
        }

        public int GetIntValue()
        {
            return db.IntValue;
        }

        public float GetFloatValue()
        {
            return db.FloatValue;
        }

        public bool GetBoolValue()
        {
            return db.BoolValue;
        }

        public string GetStringValue()
        {
            return db.StringValue;
        }
    
        private void OnValueChanged()
        {
            action?.Invoke();
        }
    }
}

