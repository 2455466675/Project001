using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace MVC
{
    public class Binder
    {
        private List<BindingHandler> handlers;

        internal Binder()
        {
            handlers = new List<BindingHandler>();
        }

        public void Binding<TSubject>(TSubject subject, string propertyName, Action<TSubject> action) where TSubject : INotifyPropertyChanged
        {
            BindingHandler handler = new BindingHandler<TSubject>(subject, propertyName, action);
            handler.Handling();
            handlers.Add(handler);
        }

        public void Binding<TSubject, TValue>(TSubject subject, Expression<Func<TSubject, TValue>> expression, Action<TSubject> action) where TSubject : INotifyPropertyChanged
        {
            if (expression.Body is not MemberExpression memberExpression)
            {
                return;
            }

            Binding(subject, memberExpression.Member.Name, action);
        }

        public void Binding<TObserver, TSubject>(TObserver observer, TSubject subject, string propertyName, Action<TObserver, TSubject> action) where TObserver : IObserver where TSubject : INotifyPropertyChanged
        {
            BindingHandler handler = new BindingHandler<TObserver, TSubject>(observer, subject, propertyName, action);
            handler.Handling();
            handlers.Add(handler);
        }

        public void Binding<TObserver, TSubject, TValue>(TObserver observer, TSubject subject, Expression<Func<TSubject, TValue>> expression, Action<TObserver, TSubject> action) where TObserver : IObserver where TSubject : INotifyPropertyChanged
        {
            if (expression.Body is not MemberExpression memberExpression)
            {
                return;
            }

            Binding(observer, subject, memberExpression.Member.Name, action);
        }

        internal void Unbinding()
        {
            foreach (var handler in handlers)
            {
                handler.Release();
            }
            handlers.Clear();
        }
    }
}
