using System;
using System.Collections.Generic;

namespace MVC
{
    /*
     * 数据树
     * 数据节点。值：数据集、列表容器、具体的数据值
     * 
    */
    public enum ValueType
    {
        None = 0,
        Int,
        Bool,
        Float,
        String,
        Container,
        Collection,
    }

    public interface IBindable
    {
        void Bind(Action action, bool isSilent);
        void Unbind(Action action);
    }

    public interface IDataModel : IBindable
    {
        ValueType ValueType { get; }
    }

    /// <summary>
    /// 
    /// </summary>
	public abstract class DataModel<T> : IDataModel where T : DataValue
    {
        protected T value;

        public abstract ValueType ValueType { get; }

        private event Action OnValueChanged;

        public void Bind(Action action, bool isSilent = false)
        {
            if (action == null)
            {
                return;
            }
            OnValueChanged += action;
            if (!isSilent)
            {
                action.Invoke();
            }
        }

        public void Unbind(Action action)
        {
            if (action == null)
            {
                return;
            }
            OnValueChanged -= action;
        }

        public void NotifyChanged()
        {
            OnValueChanged?.Invoke();
        }
    }
}

