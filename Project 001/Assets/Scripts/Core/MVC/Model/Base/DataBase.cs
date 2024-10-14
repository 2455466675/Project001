
using System;

namespace MVC
{
    /// <summary>
    /// 
    /// </summary>
	public class DataBase : DataModel<BaseValue>, IComparable<DataBase>
    {
        public override ValueType ValueType => value.ValueType;

        public DataBase(int intValue)
        {
            value = new BaseValue(NotifyChanged);
            IntValue = intValue;
        }
        public DataBase(bool boolValue)
        {
            value = new BaseValue(NotifyChanged);
            BoolValue = boolValue;
        }
        public DataBase(float floatValue)
        {
            value = new BaseValue(NotifyChanged);
            FloatValue = floatValue;
        }
        public DataBase(string stringValue)
        {
            value = new BaseValue(NotifyChanged);
            StringValue = stringValue;
        }

        public int IntValue
        {
            get
            {
                return value.IntValue;
            }
            set
            {
                this.value.IntValue = value;
            }
        }
        public bool BoolValue
        {
            get
            {
                return value.BoolValue;
            }
            set
            {
                this.value.BoolValue = value;
            }
        }
        public float FloatValue
        {
            get
            {
                return value.FloatValue;
            }
            set
            {
                this.value.FloatValue = value;
            }
        }
        public string StringValue
        {
            get
            {
                return value.StringValue;
            }
            set
            {
                this.value.StringValue = value;
            }
        }

        public override string ToString()
        {
            return StringValue;
        }

        public int CompareTo(DataBase other)
        {
            if (other.ValueType == ValueType)
            {
                if (ValueType == ValueType.Int)
                {
                    return IntValue.CompareTo(other.IntValue);
                }
                if (ValueType == ValueType.Bool)
                {
                    return BoolValue.CompareTo(other.BoolValue);
                }
                if (ValueType == ValueType.Float) 
                { 
                    return FloatValue.CompareTo(other.FloatValue);
                }
                if (ValueType == ValueType.String)
                {
                    return StringValue.CompareTo(other.StringValue);
                }            
            }
            return StringValue.CompareTo(other.StringValue);
        }
    }
}

