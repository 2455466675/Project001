using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVC
{
    public abstract class ObservableModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetValue<T>(ref T fieldValue, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(fieldValue, value))
            {
                return false;
            }
            else
            {
                fieldValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
                return true;
            }
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        public void NotifyChange() 
        {
            OnPropertyChanged(new PropertyChangedEventArgs(""));
        }
    }
}
