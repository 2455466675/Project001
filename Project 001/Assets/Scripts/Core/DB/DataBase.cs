using System;

namespace Game
{
    public interface IDataBase
    {
        void AddEvent(Action action);
        void RemoveEvent(Action action);
        void NotifyChange();
    }

    public abstract class DataBase : IDataBase
    {
        public int IntValue { get; protected set; }
        public float FloatValue { get; protected set; }
        public double DoubleValue { get; protected set; }
        public bool BoolValue { get; protected set; }
        public string StringValue { get; protected set; }

        protected Action Evts;
        public void NotifyChange()
        {
            Evts?.Invoke();
        }

        public void AddEvent(Action action)
        {
            if (action == null)
            {
                return;
            }
            Evts += action;
        }

        public void RemoveEvent(Action action)
        {
            if (action == null)
            {
                return;
            }
            Evts -= action;
        }
    }
}

