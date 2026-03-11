using System.Collections.Generic;

namespace FSM
{
    public class Blackboard : IBlackboard
    {
        private enum ValueType
        {
            Undefined,
            Integer,
            Float,
            Boolean,
            String,
        }

        private class BlackboardValue
        {
            private ValueType valueType;
            private int intValue;
            private float floatValue;
            private bool boolValue;
            private string stringValue;

            public int IntValue
            {
                get
                {
                    if (valueType == ValueType.Integer)
                    {
                        return intValue;
                    }
                    if (valueType == ValueType.Boolean)
                    {
                        return 0;
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
                    SetValue(value);
                }
            }

            public float FloatValue
            {
                get
                {
                    if (valueType == ValueType.Integer)
                    {
                        return intValue;
                    }
                    if (valueType == ValueType.Float)
                    {
                        return floatValue;
                    }
                    if (valueType == ValueType.Boolean)
                    {
                        return 0f;
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
                    SetValue(value);
                }
            }

            public bool BoolValue
            {
                get
                {
                    if (valueType == ValueType.Integer)
                    {
                        return false;
                    }
                    if (valueType == ValueType.Boolean)
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
                    SetValue(value);
                }
            }

            public string StringValue
            {
                get
                {
                    if (valueType == ValueType.Integer)
                    {
                        return intValue.ToString();
                    }
                    if (valueType == ValueType.Boolean)
                    {
                        return boolValue.ToString();
                    }
                    if (valueType == ValueType.String)
                    {
                        return stringValue.ToString();
                    }
                    return string.Empty;
                }
                set
                {
                    SetValue(value);
                }
            }

            public override string ToString()
            {
                if (valueType == ValueType.String)
                {
                    return stringValue;
                }
                if (valueType == ValueType.Integer)
                {
                    return intValue.ToString();
                }
                if (valueType == ValueType.Float)
                {
                    return floatValue.ToString();
                }
                if (valueType == ValueType.Boolean)
                {
                    return boolValue.ToString();
                }
                return string.Empty;
            }

            public BlackboardValue()
            {
                valueType = ValueType.Undefined;
                intValue = 0;
                floatValue = 0f;
                boolValue = false;
                stringValue = string.Empty;
            }

            public void SetValue(int value)
            {
                if (valueType != ValueType.Integer)
                {
                    valueType = ValueType.Integer;
                    intValue = value;
                    boolValue = false;
                    stringValue = string.Empty;
                }
                else
                {
                    if (intValue != value)
                    {
                        intValue = value;
                    }
                }
            }
            public void SetValue(float value)
            {
                if (valueType != ValueType.Float)
                {
                    valueType = ValueType.Float;
                    floatValue = value;
                    intValue = 0;
                    boolValue = false;
                    stringValue = string.Empty;
                }
                else
                {
                    if (floatValue != value)
                    {
                        floatValue = value;
                    }
                }
            }
            public void SetValue(bool value)
            {
                if (valueType != ValueType.Boolean)
                {
                    valueType = ValueType.Boolean;
                    intValue = 0;
                    floatValue = 0f;
                    boolValue = value;
                    stringValue = string.Empty;
                }
                else
                {
                    if (boolValue != value)
                    {
                        boolValue = value;
                    }
                }
            }
            public void SetValue(string value)
            {
                if (valueType != ValueType.String)
                {
                    valueType = ValueType.String;
                    intValue = 0;
                    floatValue = 0f;
                    boolValue = false;
                    stringValue = value;
                }
                else
                {
                    if (!string.Equals(stringValue, value))
                    {
                        stringValue = value;
                    }
                }
            }
        }

        private Dictionary<string, BlackboardValue> values;

        public Blackboard()
        {
            values = new Dictionary<string, BlackboardValue>();
        }

        public int GetIntValue(string key)
        {
            BlackboardValue value = GetValue(key);
            return value.IntValue;
        }
        public float GetFloatValue(string key)
        {
            BlackboardValue value = GetValue(key);
            return value.FloatValue;
        }
        public bool GetBoolValue(string key)
        {
            BlackboardValue value = GetValue(key);
            return value.BoolValue;
        }

        public string GetStringValue(string key)
        {
            BlackboardValue value = GetValue(key);
            return value.StringValue;
        }

        public void SetValue(string key, int value)
        {
            GetValue(key).SetValue(value);
        }

        public void SetValue(string key, float value)
        {
            GetValue(key).SetValue(value);
        }

        public void SetValue(string key, bool value)
        {
            GetValue(key).SetValue(value);
        }

        public void SetValue(string key, string value)
        {
            GetValue(key).SetValue(value);
        }

        private BlackboardValue GetValue(string key)
        {
            if (values.ContainsKey(key))
            {
                return values[key];
            }
            else
            {
                BlackboardValue value = new BlackboardValue();
                values.Add(key, value);
                return value;
            }
        }
    }
}