using GameFramework.Logic;
using MVC;
using static Codice.CM.WorkspaceServer.WorkspaceTreeDataStore;

namespace GameFramework.View.UI
{
    public interface INavigationController
    {
        void Show(NavigationView view, object content);

        void Hide();

        void Move(float h, float v);

        void Submit();

        bool InFocus(bool isRefocus, int[] indexs);

        void OutFocus();

        void Exit();

        bool CheckIsLocked();
    }

    public abstract class NavigationController<T> : Controller, INavigationController where T : DataModel
    {
        private NavigationView view;
        private Logic.DataModelList<T> datas;
        private object content;

        protected void SetData(Logic.DataModelList<T> datas)
        {
            if (view == null)
            {
                return;
            }
            datas ??= new Logic.DataModelList<T>();
            this.datas = datas;

            this.Binder.Binding(this, datas, d => d.Count, (c, d) => 
            {
                c.view.UpdateDataCount(datas.Count);
            });   
        }

        #region INavigationController

        void INavigationController.Show(NavigationView view, object content)
        {
            this.view = view;
            this.content = content;
            Register();
            RegisterData();
            OnShow();
        }

        void INavigationController.Hide() 
        {
            OnHide();
            Unregister();
            ClearBinding();
            view = null;
            datas = null;
            content = null;
        }

        void INavigationController.Move(float h, float v)
        {
            if (view == null)
            {
                return;
            }
            view.Move(h, v);
        }

        void INavigationController.Submit()
        {
            if (view == null)
            {
                return;
            }
            view.Submit();
        }

        bool INavigationController.InFocus(bool isRefocus, int[] indexs)
        {
            if (view == null)
            {
                return false;
            }
            return view.InFocus(isRefocus, indexs);
        }

        void INavigationController.OutFocus()
        {
            if (view == null)
            {
                return;
            }
            view.OutFocus();
        }

        void INavigationController.Exit()
        {
            if (view == null)
            {
                return;
            }
            view.Exit();
        }

        bool INavigationController.CheckIsLocked()
        {
            return CheckIsLocked();
        }

        #endregion

        #region NavigationView

        private bool CheckDataValid(int dataIndex)
        {
            bool result = datas != null && dataIndex >= 0 && dataIndex < datas.Count;
            if (!result)
            {
                MDebug.Error("数据无效 : ", dataIndex);
            }
            return result;
        }

        private void OnBindData(NavigationItemView itemView, int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return;
            }
            Binder binder = CreateBinder(itemView.GetInstanceID());
            BindItemView(itemView, datas[dataIndex], binder);
        }

        private void OnUnbindData(NavigationItemView itemView, int dataIndex)
        {
            RemoveBinder(itemView.GetInstanceID());
        }

        private void OnSelect(NavigationItemView itemView, int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return;
            }

            T dataModel = datas[dataIndex];
            SelectItemView(itemView, dataModel);
        }

        private void OnDeselect(NavigationItemView itemView, int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return;
            }

            T dataModel = datas[dataIndex];
            DeselectItemView(itemView, dataModel);
        }

        private void OnSubmit(NavigationItemView itemView, int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return;
            }

            T dataModel = datas[dataIndex];
            SubmitItemView(itemView, dataModel);
        }

        private void OnMoveUp(NavigationItemView itemView, int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return;
            }

            T dataModel = datas[dataIndex];
            MoveUpItemView(itemView, dataModel);
        }

        private void OnMoveDown(NavigationItemView itemView, int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return;
            }

            T dataModel = datas[dataIndex];
            MoveDownItemView(itemView, dataModel);
        }

        private void OnMoveLeft(NavigationItemView itemView, int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return;
            }

            T dataModel = datas[dataIndex];
            MoveLeftItemView(itemView, dataModel);
        }

        private void OnMoveRight(NavigationItemView itemView, int dataIndex)
        {
            if (!CheckDataValid(dataIndex))
            {
                return;
            }

            T dataModel = datas[dataIndex];
            MoveRightItemView(itemView, dataModel);
        }

        private bool CheckValid(int dataIndex) 
        {
            if (!CheckDataValid(dataIndex))
            {
                return false;
            }
            T dataModel = datas[dataIndex];
            return CheckItemIsValid(dataModel);
        }

        #endregion

        #region 生命周期
        /// <summary>
        /// 在这里获取列表需要的数据 （该方法在OnShow之前执行）
        /// </summary>
        protected virtual void RegisterData() { }
        protected virtual void OnShow() { }
        protected virtual void OnHide() { }
        protected virtual void BindItemView(NavigationItemView itemView, T dataModel, Binder binder) { }
        protected virtual void SelectItemView(NavigationItemView itemView, T dataModel) { }
        protected virtual void DeselectItemView(NavigationItemView itemView, T dataModel) { }
        protected virtual void SubmitItemView(NavigationItemView itemView, T dataModel) { }
        protected virtual void MoveUpItemView(NavigationItemView itemView, T dataModel) { }
        protected virtual void MoveDownItemView(NavigationItemView itemView, T dataModel) { }
        protected virtual void MoveLeftItemView(NavigationItemView itemView, T dataModel) { }
        protected virtual void MoveRightItemView(NavigationItemView itemView, T dataModel) { }

        #endregion

        /// <summary>
        /// 列表项数据是否有效
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        protected virtual bool CheckItemIsValid(T dataModel) 
        {
            return true;
        }

        /// <summary>
        /// 列表是否锁定
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckIsLocked()
        {
            return false;
        }

        protected TContent GetContent<TContent>()
        {
            if (content == null)
            {
                return default;
            }
            else
            {
                if (content is TContent result)
                {
                    return result;
                }
                else
                {
                    return default;
                }                
            }
        }

        private void Register() 
        {
            if (view == null) 
            {
                return;
            }

            view.OnSelectItem += OnSelect;
            view.OnDeselectItem += OnDeselect;
            view.OnSubmitItem += OnSubmit;
            view.OnMoveUpItem += OnMoveUp;
            view.OnMoveDownItem += OnMoveDown;
            view.OnMoveLeftItem += OnMoveLeft;
            view.OnMoveRigthItem += OnMoveRight;
            view.OnItemBindData += OnBindData;
            view.OnItemUnbindData += OnUnbindData;
            view.OnCheckItemValid += CheckValid;
        }

        private void Unregister()
        {
            if (view == null)
            {
                return;
            }

            view.OnSelectItem -= OnSelect;
            view.OnDeselectItem -= OnDeselect;
            view.OnSubmitItem -= OnSubmit;
            view.OnMoveUpItem -= OnMoveUp;
            view.OnMoveDownItem -= OnMoveDown;
            view.OnMoveLeftItem -= OnMoveLeft;
            view.OnMoveRigthItem -= OnMoveRight;
            view.OnItemBindData -= OnBindData;
            view.OnItemUnbindData -= OnUnbindData;
            view.OnCheckItemValid -= CheckValid;
        }
    }
}
