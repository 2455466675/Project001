using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameFramework 
{
    [Serializable]
    public sealed class DataModel : IDataModel
    {
        private enum ValueType 
        {
            Undefined,
            Integer,
            Float,
            Boolean,
            String,
        }

        [Serializable]
        private class DataModelValue
        {
            [SerializeField]
            private ValueType m_ValueType;
            [SerializeField]
            private int m_IntValue;
            [SerializeField]
            private float m_FloatValue;
            [SerializeField]
            private bool m_BoolValue;
            [SerializeField]
            private string m_StringValue;

            public int IntValue
            {
                get 
                {
                    if (m_ValueType == ValueType.Integer) 
                    {
                        return m_IntValue;
                    }
                    if (m_ValueType == ValueType.Boolean) 
                    {
                        return 0;
                    }
                    if (m_ValueType == ValueType.String) 
                    {
                        if(int.TryParse(m_StringValue, out int v)) 
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
                    if (m_ValueType == ValueType.Integer)
                    {
                        return m_IntValue;
                    }
                    if (m_ValueType == ValueType.Float)
                    {
                        return m_FloatValue;
                    }
                    if (m_ValueType == ValueType.Boolean)
                    {
                        return 0f;
                    }
                    if (m_ValueType == ValueType.String)
                    {
                        if (float.TryParse(m_StringValue, out float v))
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
                    if (m_ValueType == ValueType.Integer)
                    {
                        return false;
                    }
                    if (m_ValueType == ValueType.Boolean)
                    {
                        return m_BoolValue;
                    }
                    if (m_ValueType == ValueType.String)
                    {
                        if (bool.TryParse(m_StringValue, out bool v))
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
                    if (m_ValueType == ValueType.Integer)
                    {
                        return m_IntValue.ToString();
                    }
                    if (m_ValueType == ValueType.Boolean)
                    {
                        return m_BoolValue.ToString();
                    }
                    if (m_ValueType == ValueType.String)
                    {
                        return m_StringValue.ToString();
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
                if (m_ValueType == ValueType.String) 
                {
                    return m_StringValue;                    
                }
                if (m_ValueType == ValueType.Integer)
                {
                    return m_IntValue.ToString();
                }
                if (m_ValueType == ValueType.Float)
                {
                    return m_FloatValue.ToString();
                }
                if (m_ValueType == ValueType.Boolean)
                {
                    return m_BoolValue.ToString();
                }
                return string.Empty;
            }

            public DataModelValue() 
            {
                m_ValueType = ValueType.Undefined;
                m_IntValue = 0;
                m_FloatValue = 0f;
                m_BoolValue = false;
                m_StringValue = string.Empty;
            }

            public bool SetValue(int value)
            {
                bool isChanged = false;
                if (m_ValueType != ValueType.Integer)
                {
                    m_ValueType = ValueType.Integer;
                    m_IntValue = value;
                    m_BoolValue = false;
                    m_StringValue = string.Empty;
                    isChanged = true;
                }
                else
                {
                    if (m_IntValue != value) 
                    {
                        m_IntValue = value;
                        isChanged = true;
                    }
                }
                return isChanged;
            }
            public bool SetValue(float value)
            {
                bool isChanged = false;
                if (m_ValueType != ValueType.Float)
                {
                    m_ValueType = ValueType.Float;
                    m_FloatValue = value;
                    m_IntValue = 0;
                    m_BoolValue = false;
                    m_StringValue = string.Empty;
                    isChanged = true;
                }
                else
                {
                    if (m_FloatValue != value)
                    {
                        m_FloatValue = value;
                        isChanged = true;
                    }
                }
                return isChanged;
            }
            public bool SetValue(bool value)
            {
                bool isChanged = false;
                if (m_ValueType != ValueType.Boolean)
                {
                    m_ValueType = ValueType.Boolean;
                    m_IntValue = 0;
                    m_FloatValue = 0f;
                    m_BoolValue = value;
                    m_StringValue = string.Empty;
                    isChanged = true;
                }
                else
                {
                    if (m_BoolValue != value)
                    {
                        m_BoolValue = value;
                        isChanged = true;
                    }
                }
                return isChanged;
            }

            public bool SetValue(string value)
            {
                bool isChanged = false;
                if (m_ValueType != ValueType.String)
                {
                    m_ValueType = ValueType.String;
                    m_IntValue = 0;
                    m_FloatValue = 0f;
                    m_BoolValue = false;
                    m_StringValue = value;
                    isChanged = true;
                }
                else
                {
                    if (!string.Equals(m_StringValue, value))
                    {
                        m_StringValue = value;
                        isChanged = true;
                    }
                }
                return isChanged;
            }                         
        }

        [SerializeField]
        private Dictionary<string, DataModelValue> m_Values;

        public event Action<string> OnValueChanged;

        public DataModel() 
        {
            m_Values = new Dictionary<string, DataModelValue>();
        }

        public void SetValue(string key, int value) 
        {
            DataModelValue modelValue = GetValue(key);
            if (modelValue.SetValue(value)) 
            {
                OnValueChanged?.Invoke(key);            
            }
        }
        public void SetValue(string key, float value)
        {
            DataModelValue modelValue = GetValue(key);
            if (modelValue.SetValue(value))
            {
                OnValueChanged?.Invoke(key);
            }
        }
        public void SetValue(string key, bool value)
        {
            DataModelValue modelValue = GetValue(key);
            if (modelValue.SetValue(value))
            {
                OnValueChanged?.Invoke(key);
            }
        }

        public void SetValue(string key, string value)
        {
            DataModelValue modelValue = GetValue(key);
            if (modelValue.SetValue(value))
            {
                OnValueChanged?.Invoke(key);
            }
        }

        public int GetIntValue(string key) 
        {
            DataModelValue modelValue = GetValue(key);
            return modelValue.IntValue;
        }
        public float GetFloatValue(string key)
        {
            DataModelValue modelValue = GetValue(key);
            return modelValue.FloatValue;
        }
        public bool GetBoolValue(string key)
        {
            DataModelValue modelValue = GetValue(key);
            return modelValue.BoolValue;
        }

        public string GetStringValue(string key)
        {
            DataModelValue modelValue = GetValue(key);
            return modelValue.StringValue;
        }

        private DataModelValue GetValue(string key) 
        {
            if (m_Values.ContainsKey(key)) 
            {
                return m_Values[key];
            }
            else
            {
                DataModelValue modelValue = new DataModelValue();
                m_Values.Add(key, modelValue);
                return modelValue;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            sb.AppendLine();
            foreach (var item in m_Values)
            {
                sb.Append("\t");
                sb.Append(item.Key);
                sb.Append(":");
                sb.Append(item.Value);
                sb.AppendLine();
            }
            sb.AppendLine();
            sb.Append("}");

            return sb.ToString();
        }
    }

    public class DataModelWrapper : IDataModel
    {
        private readonly DataModel m_Data = new DataModel();

        public DataModel GetDataModel()
        {
            return m_Data;
        }

        public bool GetBoolValue(string key)
        {
            return m_Data.GetBoolValue(key);
        }

        public float GetFloatValue(string key)
        {
            return m_Data.GetFloatValue(key);
        }

        public int GetIntValue(string key)
        {
            return m_Data.GetIntValue(key);
        }

        public string GetStringValue(string key)
        {
            return m_Data.GetStringValue(key);
        }

        public void SetValue(string key, int value)
        {
            m_Data.SetValue(key, value);
        }

        public void SetValue(string key, float value)
        {
            m_Data.SetValue(key, value);
        }

        public void SetValue(string key, bool value)
        {
            m_Data.SetValue(key, value);
        }

        public void SetValue(string key, string value)
        {
            m_Data.SetValue(key, value);
        }
    }
}