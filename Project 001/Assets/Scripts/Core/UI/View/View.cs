using Game.Core;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Game.UI
{
    public class ViewField
    {
        public bool IsValid => db != null;
        public int IntValue => IsValid ? db.IntValue : 0;
        public float FloatValue => IsValid ? db.FloatValue : 0f;
        public double DoubleValue => IsValid ? db.DoubleValue : 0d;
        public bool BoolValue => IsValid && db.BoolValue;
        public string StringValue => IsValid ? db.StringValue : string.Empty;
        public List<ItemDB> ListValue => db?.ListValue();

        private View view;
        private IDataBase db;
        public ViewField(View view, IDataBase db)
        {
            this.view = view;
            this.db = db;

            if (db == null )
            {
                return;
            }

            db.AddEvent(OnValueChanged);
        }

        public void OnDestory()
        {
            db?.RemoveEvent(OnValueChanged);
        }

        private void OnValueChanged()
        {
            if (view == null) return;            
            view.OnDatumChange();
        }

        public override string ToString()
        {
            return db?.ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class View : MonoBehaviour
    {
        private ViewField[] fields;
        private Action onUpateView;
        private Action onUpatedView;

        public string vUid;
        /// <summary>
        /// 延迟刷新脏标记
        /// </summary>
        public bool IsDirty { get; private set;}

        public ViewField this[int index]
        {
            get
            {
                if (fields == null || index < 0 || fields.Length <= index)
                {
                    return null;
                }
                return fields[index];
            }
        }

        /// <summary>
        /// 字段数
        /// </summary>
        public int FieldCount => fields != null ? fields.Length : 0;

        /// <summary>
        /// 主字段
        /// </summary>
        public ViewField MainField => this[0];

        /// <summary>
        /// 视图组件是否有效
        /// </summary>
        public bool IsValid => MainField != null && MainField.IsValid;

        public virtual void OnEnable()
        {
            if (IsDirty)
            {
                OnDatumChange();
            }
        }

        public virtual void OnDisable()
        {
            IsDirty = false;
        }

        public virtual void OnDestroy()
        {
            Clear();
        }

        public void SetDatum(params IDataBase[] datums)
        {
            Clear();
            if(datums == null || datums.Length <= 0)
            {
                return;
            }
            else
            {
                fields = new ViewField[datums.Length];              
                for(int i = 0; i < datums.Length; i++) 
                {                    
                    fields[i] = new ViewField(this, datums[i]);                    
                }
                OnDatumChange();
            }
        }

        public void OnDatumChange()
        {
            if (gameObject.activeInHierarchy) 
            {
                onUpateView?.Invoke();
                UpdateView();
                onUpatedView?.Invoke();
                IsDirty = false;
            }
            else
            {
                IsDirty = true;
            }
        }

        /// <summary>
        /// 更新视图
        /// </summary>
        public virtual void UpdateView()
        {
        }

        public void AddOnUpdateViewListener(Action action)
        {
            if (action == null)
            {
                return;
            }
            onUpateView += action;
        }

        public void RemoveOnUpdateViewListener(Action action)
        {
            if (action == null)
            {
                return;
            }
            onUpateView -= action;
        }

        public void AddOnUpdatedViewListener(Action action)
        {
            if (action == null)
            {
                return;
            }
            onUpatedView += action;
        }

        public void RemoveOnUpdatedViewListener(Action action)
        {
            if (action == null)
            {
                return;
            }
            onUpatedView -= action;
        }

        private void Clear()
        {
            if (fields == null || fields.Length <= 0) return;

            for (int i = 0; i < fields.Length; i++) 
            {
                fields[i].OnDestory();
                fields[i] = null;               
            }
            fields = null;
        }

        [Button("Print Data")]
        private void PrintData()
        {
            PrintDataInner();
        }

        protected void PrintDataInner()
        {
            if (fields == null || fields.Length <= 0)
            {
                MLog.Log("null"); 
                return;
            }

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < fields.Length; i++) 
            {
                stringBuilder.AppendLine($" field_{i} : {fields[i]} ");
            }

            MLog.Log(stringBuilder.ToString());
        }
    }
}