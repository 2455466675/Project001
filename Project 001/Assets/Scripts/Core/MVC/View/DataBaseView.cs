using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MVC
{
    /// <summary>
    /// 
    /// </summary>
	public class DataBaseView : View
	{
        [SerializeField]
        private string[] fields;
        public int FieldCount => fields != null ? fields.Length : 0;
        public int Count => viewFields != null ? viewFields.Length : 0;
        public DataBaseViewField MainField => viewFields?[0];
        public DataBaseViewField this[int index] => viewFields?[index];
        private DataBaseViewField[] viewFields;
   
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Clear();
            fields = null;
        }

        public override void UpdateViewField()
        {
            if (dataSet == null)
            {
                return;
            }
            if (fields == null || fields.Length == 0)
            {
                return;
            }

            List<DataBase> dbs = new List<DataBase>();
            for (int i = 0; i < fields.Length; i++)
            {
                DataBase db = dataSet.FindDataBase(fields[i]);
                if (db == null)
                {
                    continue;
                }

                dbs.Add(db);
            }

            Clear();

            viewFields = new DataBaseViewField[dbs.Count];
            for (int i = 0; i < dbs.Count; i++)
            {
                viewFields[i] = new DataBaseViewField(dbs[i]);
            }

            for (int i = 0; i < viewFields.Length; i++)
            {
                viewFields[i].Bind(OnValueChanged);
            }
        }

        private void Clear()
        {
            if (viewFields != null)
            {
                foreach (var field in viewFields)
                {
                    field.Unbind();
                }
            }
            viewFields = null;
        }

        [Button("Print")]
        private void Print()
        {
            if (Count == 0)
            {
                Debug.Log("null");
                return;
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < viewFields.Length; i++)
            {
                builder.Append(viewFields[i].ToString());
            }
            Debug.Log(builder.ToString());
        }
    }
}

