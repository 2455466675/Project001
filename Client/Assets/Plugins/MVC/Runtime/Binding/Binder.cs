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

        public void Binding<TS>(TS subject, string propertyName, Action<TS> action) where TS : INotifyPropertyChanged
        {
            BindingHandler handler = new BindingHandler<TS>(subject, propertyName, action);
            handler.Handling();
            handlers.Add(handler);
        }

        public void Binding<TS, TValue>(TS subject, Expression<Func<TS, TValue>> expression, Action<TS> action) where TS : INotifyPropertyChanged
        {
            if (expression.Body is not MemberExpression memberExpression)
            {
                return;
            }

            Binding(subject, memberExpression.Member.Name, action);
        }

        public void Binding<TO, TS>(TO observer, TS subject, string propertyName, Action<TO, TS> action) where TS : INotifyPropertyChanged
        {
            BindingHandler handler = new BindingHandler<TO, TS>(observer, subject, propertyName, action);
            handler.Handling();
            handlers.Add(handler);
        }

        public void Binding<TO, TS, TValue>(TO observer, TS subject, Expression<Func<TS, TValue>> expression, Action<TO, TS> action) where TS : INotifyPropertyChanged
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
