using Game.Core;
using System.Collections.Generic;
using System.Linq;

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
	public class ListView : View, IGuidableGroup
    {
        public NavigationBox box;     
        public int Layer { get; set;}
        public bool IsFocus {get; private set;}
        public ListViewState ViewState {get; private set;}
        public ListDB ListDB => IsValid ? MainDB as ListDB : null;
        public int Count => items != null ? items.Count : 0;

        protected List<ItemDB> items;

        public virtual void Awake()
        {
            if(box == null)
            {
                box = gameObject.GetComponent<NavigationBox>();
            }
            if(box != null)
            {
                box.SetView(this);
            }
            ViewState = ListViewState.Exit;
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
            if (ListDB == null || ListDB.Count <= 0)
            {
                items?.Clear();
                return;
            }
            items = ListDB.Where(a => a.Filter()).OrderBy(a => a).ToList();
        }

        public void InFocus()
        {
            if (ViewState == ListViewState.Exit)
            {
                box.SelectDefault();
            }
            else
            {
                //TODO 选择失焦之前选择的
                MLog.Log("选择失焦之前选择的");
                box.Select(box.currIndex);
            }
            ViewState = ListViewState.InFocus;
            IsFocus = true;
        }

        public void OutFocus()
        {
            ViewState = ListViewState.OutFocus;
            IsFocus = false;
        }

        public void Exit()
        {
            ViewState = ListViewState.Exit;
            IsFocus = false;
        }

        public void OnMoveToUp()
        {
            if (box == null) 
            {
                return;
            }
            box.MoveUp();                
        }

        public void OnMoveToDown()
        {
            if (box == null)
            {
                return;
            }
            box.MoveDown();
        }

        public void OnMoveToLeft()
        {
            if (box == null)
            {
                return;
            }
            box.MoveLeft();
        }

        public void OnMoveToRight()
        {
            if (box == null)
            {
                return;
            }
            box.MoveRight();
        }
    }
}

