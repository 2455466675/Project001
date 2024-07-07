using Game.Core;
using Sirenix.OdinInspector;
using System.Text;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class View : MonoBehaviour
    {
        public string vUid;

        /// <summary>
        /// 延迟刷新脏标记
        /// </summary>
        public bool IsDirty { get; private set;}

        public IDataBase[] DBs {get; private set;}

        public IDataBase MainDB => (DBs != null && DBs.Length > 0) ? DBs[0] : null;

        /// <summary>
        /// 视图组件是否有效
        /// </summary>
        public bool IsValid => DBs != null && DBs.Length > 0 && MainDB != null;

        public virtual void OnEnable()
        {
            if (IsDirty)
            {
                UpdateView();
            }
        }

        public virtual void OnDisable()
        {
            IsDirty = false;
        }

        public void OnDestroy()
        {
            ClearDBs();
        }

        public void SetDatum(params IDataBase[] datums)
        {
            ClearDBs();
            if(datums == null || datums.Length <= 0)
            {
                return;
            }
            else
            {
                DBs = datums;
                for(int i = 0; i < DBs.Length; i++) 
                {
                    DBs[i].AddEvent(OnDatumChange);
                }
                OnDatumChange();
            }
        }

        /// <summary>
        /// 更新视图
        /// </summary>
        public virtual void UpdateView()
        {       
        }

        private void OnDatumChange()
        {
            if (gameObject.activeInHierarchy) 
            {
                UpdateView();
                IsDirty = false;
            }
            else
            {
                IsDirty = true;
            }
        }
        
        private void ClearDBs()
        {
            if (DBs == null || DBs.Length <= 0) return;

            for (int i = 0; i < DBs.Length; i++) 
            {
                DBs[i].RemoveEvent(OnDatumChange);
                DBs[i] = null;               
            }
            DBs = null;
        }

        [Button("Print Data")]
        private void PrintData()
        {
            PrintDataInner();
        }

        protected void PrintDataInner()
        {
            if (DBs == null || DBs.Length <= 0)
            {
                MLog.Log("null"); 
                return;
            }

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < DBs.Length; i++) 
            {
                stringBuilder.AppendLine($" DB_{i} : {DBs[i]} ");
            }

            MLog.Log(stringBuilder.ToString());
        }
    }
}