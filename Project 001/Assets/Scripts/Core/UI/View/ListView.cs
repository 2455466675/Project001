using Game.Core;
using UnityEngine;
using System.Collections.Generic;

namespace Game.UI
{
    public enum ListViewState
    {
        /// <summary>
        /// 聚焦
        /// </summary>
        InFocus  = 1, 
        /// <summary>
        /// 失焦
        /// </summary>
        OutFocus = 2,
        /// <summary>
        /// 退出
        /// </summary>
        Exit   = 3,
    }

    /// <summary>
    /// 
    /// </summary>
	public class ListView : View, INavigatable
    {
        private static Dictionary<ListViewId, ListView> views;

        public static void AddView(ListView listView)
        {
            views ??= new Dictionary<ListViewId, ListView>();

            ListViewId id = listView.Id;

            if (id == ListViewId.Undefined)
            {
                MLog.Error("未定义的列表视图：" + listView.name);
                return;
            }

            if (views.ContainsKey(id))
            {
                MLog.Error("重复的列表视图：" + id.ToString());
                return;
            }

            views[id] = listView;
        }

        public static void RemoveView(ListViewId id)
        {
            if (!views.ContainsKey(id))
            {
                return;
            }
            views.Remove(id);
        }

        public static ListView GetView(ListViewId id) 
        {
            if (!views.ContainsKey(id))
            {
                MLog.Error("没有该列表视图：" + id.ToString());
                return null;
            }
            return views[id];
        }

        public static void Register(ListViewId id, params IDataBase[] datums)
        {
            ListView view = GetView(id);
            if (view == null)
            {
                return;
            }
            view.SetDatum(datums);
        }

        [SerializeField]
        private ListViewId id;
        public ListViewId Id => id;

        public GuidableBox box;     

        public bool IsFocus => ViewState == ListViewState.InFocus;
        public ListViewState ViewState {get; private set;}

        public int Count => items != null ? items.Count : 0;

        protected List<ItemDB> items;

        private int index;

        public virtual void Awake()
        {
            AddView(this);

            if(box == null)
            {
                box = gameObject.GetComponent<GuidableBox>();
            }

            ViewState = ListViewState.Exit;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            RemoveView(Id);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Exit();
        }

        public override void UpdateView()
        {
            Query();
        }

        private void Query()
        {
            if (!IsValid)
            {
                items?.Clear();
                return;
            }

            items = MainField.ListValue;
        }

        public void InFocus()
        {
            ViewState = ListViewState.InFocus;
            box.Select(index);
        }

        public void OutFocus()
        {
            ViewState = ListViewState.OutFocus;
            index = box.CurrIndex;
        }

        public void Exit()
        {
            ViewState = ListViewState.Exit;
            index = 0;
        }

        public void MoveUp()
        {
            if (box == null) 
            {
                return;
            }
            box.Move(MoveType.Up);
        }

        public void MoveDown()
        {
            if (box == null)
            {
                return;
            }
            box.Move(MoveType.Down);
        }

        public void MoveLeft()
        {
            if (box == null)
            {
                return;
            }
            box.Move(MoveType.Left);
        }

        public void MoveRight()
        {
            if (box == null)
            {
                return;
            }
            box.Move(MoveType.Right);
        }
    }
}

