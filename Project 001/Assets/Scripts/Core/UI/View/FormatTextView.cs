using System;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class FormatTextView : TextView
	{
        [Header("原始字段")]
        [Tooltip("对应字段不是语言id,直接当作格式化参数")]
        [SerializeField]
        private bool[] rawFields;

        protected override void OnUpdateView()
        {
            if (textId > 0)
            {
                string text = GameCore.Cfg.GetTextById(textId);
                string[] args = new string[Count];
                for (int i = 0; i < Count; i++)
                {
                    if (IsRaw(i))
                    {
                        args[i] = this[i].GetStringValue();
                    }
                    else
                    {
                        args[i] = GameCore.Cfg.GetTextById(this[i].GetIntValue());
                    }
                }
                SetTextByStr(string.Format(text, args));
            }
            else
            {
                int id = MainField.GetIntValue();                
                string text = (id > 0 && !IsRaw(0)) ? GameCore.Cfg.GetTextById(id) : MainField.GetStringValue();

                if (Count == 1)
                {
                    SetTextByStr(text);
                }
                else
                {
                    string[] args = new string[Count - 1];
                    for (int i = 1; i < Count; i++)
                    {                        
                        if (IsRaw(i))
                        {
                            args[i - 1] = this[i].GetStringValue();
                        }
                        else
                        {
                            args[i - 1] = GameCore.Cfg.GetTextById(this[i].GetIntValue());
                        }
                    }
                    SetTextByStr(string.Format(text, args));
                }
            }
        }

        /// <summary>
        /// 是否保留原始值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool IsRaw(int index)
        {
            if (rawFields == null)
            {
                return false;
            }

            if (index < 0 || index >= rawFields.Length)
            {
                return false;
            }
                
            return rawFields[index];
        }

#if UNITY_EDITOR
        public override void OnValidate()
        {
            base.OnValidate();
    
            if (rawFields == null)
                rawFields = new bool[FieldCount];

            if (rawFields.Length != FieldCount)
                Array.Resize(ref rawFields, FieldCount);
        }
#endif
    }
}

