using System;
using System.ComponentModel;

namespace MVC
{
    internal abstract class BindingHandler
    {
        private INotifyPropertyChanged notifier;
        private string propertyName;

        internal BindingHandler(INotifyPropertyChanged notifier, string propertyName)
        {
            this.propertyName = propertyName;
            this.notifier = notifier;
            this.notifier.PropertyChanged += HandleModelPropertyChanged;
        }

        internal void Release()
        {
            OnRelease();
            this.notifier.PropertyChanged -= HandleModelPropertyChanged;
            this.notifier = null;
        }

        internal void Handling()
        {
            BindingUpdate();
        }

        protected virtual void OnRelease()
        {

        }

        protected virtual void BindingUpdate()
        {

        }

        private void HandleModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName) || string.Equals(e.PropertyName, propertyName))
            {
                Handling();
            }
        }
    }

    internal class BindingHandler<TS> : BindingHandler where TS : INotifyPropertyChanged 
    {
        private TS subject;
        private Action<TS> propertyChangedAction;

        internal BindingHandler(TS subject, string propertyName, Action<TS> propertyChangedAction) : base(subject, propertyName)
        {
            this.subject = subject;
            this.propertyChangedAction = propertyChangedAction;
        }
        protected override void OnRelease()
        {
            subject = default(TS);
            propertyChangedAction = null;
        }

        protected override void BindingUpdate()
        {
            propertyChangedAction?.Invoke(subject);
        }
    }

    internal class BindingHandler<TO, TS> : BindingHandler where TS : INotifyPropertyChanged
    {
        private TO observer;
        private TS subject;
        private Action<TO, TS> propertyChangedAction;

        internal BindingHandler(TO observer, TS subject, string propertyName, Action<TO, TS> propertyChangedAction) : base(subject, propertyName)
        {
            this.observer = observer;
            this.subject = subject;
            this.propertyChangedAction = propertyChangedAction;
        }

        protected override void OnRelease()
        {
            observer = default(TO);
            subject = default(TS);
            propertyChangedAction = null;
        }

        protected override void BindingUpdate()
        {
            propertyChangedAction?.Invoke(observer, subject);
        }
    }
}