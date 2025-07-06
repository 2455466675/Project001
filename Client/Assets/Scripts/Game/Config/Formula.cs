using System;
using UnityEngine;

namespace Config
{
    /// <summary>
    /// 游戏数据定义
    /// </summary>
    [CreateAssetMenu(menuName= "MyMenu/Create Formula")]
	public class Formula : ScriptableObject
	{
        [Serializable]
        private class FormulaItem 
        {
            public string desc;
            public string key;
            public string value;
        }

        [SerializeField]
        private FormulaItem[] items;

        public string GetStringValue(string key) 
        {
            if (items == null || items.Length == 0) 
            {
                return string.Empty;   
            }

            FormulaItem item = Array.Find(items, x => x.key == key);
            if (item == null) 
            {
                return string.Empty;
            }

            return item.value;
        }

        public int GetIntValue(string key) 
        {
            if (items == null || items.Length == 0)
            {
                return 0;
            }

            FormulaItem item = Array.Find(items, x => x.key == key);
            if (item == null)
            {
                return 0;
            }

            if (int.TryParse(item.value, out int value)) 
            {
                return value;
            }
            else
            {
                return 0;
            }           
        }

        public float GetFloatValue(string key)
        {
            if (items == null || items.Length == 0)
            {
                return 0f;
            }

            FormulaItem item = Array.Find(items, x => x.key == key);
            if (item == null)
            {
                return 0f;
            }

            if (float.TryParse(item.value, out float value))
            {
                return value;
            }
            else
            {
                return 0f;
            }
        }

        public bool GetBoolValue(string key)
        {
            if (items == null || items.Length == 0)
            {
                return false;
            }

            FormulaItem item = Array.Find(items, x => x.key == key);
            if (item == null)
            {
                return false;
            }

            if (bool.TryParse(item.value, out bool value))
            {
                return value;
            }
            else
            {
                return false;
            }
        }
    }
}

