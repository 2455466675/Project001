using System;

namespace MVC
{
    /// <summary>
    /// 
    /// </summary>
	public class BaseValue : DataValue, IBindable
	{
        public ValueType ValueType => valueType;
        private ValueType valueType;
        private int intValue;
        private bool boolValue;
        private float floatValue;
        private string stringValue;
        private event Action OnValueChanged;

        public BaseValue(Action onValueChanged)
        {
            Bind(onValueChanged);
        }

        public int IntValue
        { 
            get
            {
                if (valueType == ValueType.Int)
                {
                    return intValue;
                }
                if (valueType == ValueType.Float)
                {
                    return (int)floatValue;
                }
                if (valueType == ValueType.String)
                {
                    if (int.TryParse(stringValue, out int v))
                    {
                        return v;
                    }
                }
                return 0;
            }
            set
            {
                if(valueType == ValueType.Int && intValue == value)
                {
                    return;                    
                }
                intValue = value;
                NotifyChanged();
                valueType = ValueType.Int;
            }
        }
        
        public bool BoolValue 
        {
            get
            {
                if (valueType == ValueType.Bool)
                {
                    return boolValue;
                }
                if (valueType == ValueType.String)
                {
                    if (bool.TryParse(stringValue, out bool v))
                    {
                        return v;
                    }
                }
                return false;
            }
            set
            {
                if (valueType == ValueType.Bool && boolValue == value)
                {
                    return;
                }
                boolValue = value;
                NotifyChanged();
                valueType = ValueType.Bool;
            }
        }

        public float FloatValue 
        {
            get
            {
                if (valueType == ValueType.Float)
                {
                    return floatValue;
                }
                if (valueType == ValueType.Int)
                {
                    return intValue;
                }
                if (valueType == ValueType.String)
                {
                    if (float.TryParse(stringValue, out float v))
                    {
                        return v;
                    }
                }
                return 0f;
            }
            set
            {
                if (valueType == ValueType.Float && floatValue == value)
                {
                    return;
                }
                floatValue = value;
                NotifyChanged();
                valueType = ValueType.Float;
            }
        }
        
        public string StringValue 
        {
            get
            {
                if (valueType == ValueType.String)
                {
                    return stringValue;
                }
                if (valueType == ValueType.Int)
                {
                    return intValue.ToString();
                }
                if (valueType == ValueType.Float)
                {
                    return floatValue.ToString();
                }
                if (valueType == ValueType.Bool)
                {
                    return boolValue.ToString();
                }
                return string.Empty;
            }
            set
            {
                if (valueType == ValueType.String && string.Equals(stringValue, value))
                {
                    return;
                }
                stringValue = value;
                NotifyChanged();
                valueType = ValueType.String;
            }
        }

        public void Bind(Action action, bool isSilent = true)
        {
            if (action == null)
            {
                return;
            }
            OnValueChanged += action;
            if (!isSilent)
            {
                action.Invoke();
            }
        }

        public void Unbind(Action action)
        {
            if (action == null)
            {
                return;
            }
            OnValueChanged -= action;
        }

        private void NotifyChanged()
        {
            OnValueChanged?.Invoke();
        }
    }
}

